using CRMWebService.Infrastructure.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace CRMWebService.Repository
{
    public class OutstandingRepository : IOutstandingRepository
    {
        public Domain.OutstandingLister GetAll(Domain.OutstandingLister mLister)
        {
            try
            {
                using (var context = new Persistence.CRMDBEntities())
                {
                    var query = from u in context.Users
                                join ut in context.UserTypes on u.UserTypeId equals ut.Id
                                where ut.Id == (int)Infrastructure.Utils.Enums.UserType.Dyeing_Supervisor
                                join c in context.Clients on u.Id equals c.DyeingSupervisorId
                                join i in context.Invoices on c.Id equals i.ClientId into invoices
                                join r in context.Receipts on c.Id equals r.ClientId into receipts
                                group new { invoices, receipts } by new { u.FirstName, u.LastName } into g
                                select new
                                {
                                    DyeingSupervisor = g.Key.FirstName + " " + g.Key.LastName,
                                    Total = (g.SelectMany(x => x.invoices).Sum(i => (decimal?)i.NetAmount) ?? 0)
                                                - (g.SelectMany(x => x.receipts).Sum(r => (decimal?)r.TotalPayment) ?? 0)
                                };

                    // Switch to in-memory for complex string filters
                    var efOutstanding = query.AsEnumerable();

                    var pagination = mLister.Pagination ?? new Domain.Pagination();

                    pagination.TotalRecord = Math.Max(efOutstanding.Count(), 1);

                    if (pagination.Take <= 0)
                    {
                        pagination.Take = pagination.TotalRecord;
                        pagination.Skip = 0;
                    }

                    pagination.CurrentPage = pagination.Take > 0 ? (pagination.Skip / pagination.Take) + 1 : 1;

                    pagination.TotalPage = pagination.Take > 0 ? (int)Math.Ceiling((double)pagination.TotalRecord / pagination.Take) : 1;

                    efOutstanding = efOutstanding.Skip(pagination.Skip).Take(pagination.Take);

                    mLister.Pagination = pagination;
                    mLister.List = JsonSerializer.Deserialize<List<Domain.Outstanding>>(JsonSerializer.Serialize(efOutstanding.ToList()));
                }
            }
            catch (Exception)
            {
                throw;
            }
            return mLister;
        }
    }
}
