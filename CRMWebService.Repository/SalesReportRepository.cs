using CRMWebService.Infrastructure.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace CRMWebService.Repository
{
    public class SalesReportRepository : ISalesReportRepository
    {
        public Domain.SalesReportLister GetAll(Domain.SalesReportLister mLister)
        {
            try
            {
                using (var context = new Persistence.CRMDBEntities())
                {
                    var query = from i in context.Invoices
                                join u in context.Users on i.DyeingSupervisorId equals u.Id
                                group i by new { u.FirstName, u.LastName } into g
                                select new
                                {
                                    DyeingSupervisor = g.Key.FirstName + " " + g.Key.LastName,
                                    Total = g.Sum(x => x.NetAmount),
                                };

                    // Switch to in-memory for complex string filters
                    var efSalesReport = query.AsEnumerable();

                    var pagination = mLister.Pagination ?? new Domain.Pagination();

                    pagination.TotalRecord = Math.Max(efSalesReport.Count(), 1);

                    if (pagination.Take <= 0)
                    {
                        pagination.Take = pagination.TotalRecord;
                        pagination.Skip = 0;
                    }

                    pagination.CurrentPage = pagination.Take > 0 ? (pagination.Skip / pagination.Take) + 1 : 1;

                    pagination.TotalPage = pagination.Take > 0 ? (int)Math.Ceiling((double)pagination.TotalRecord / pagination.Take) : 1;

                    efSalesReport = efSalesReport.Skip(pagination.Skip).Take(pagination.Take);

                    mLister.Pagination = pagination;
                    mLister.List = JsonSerializer.Deserialize<List<Domain.SalesReport>>(JsonSerializer.Serialize(efSalesReport.ToList()));
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
