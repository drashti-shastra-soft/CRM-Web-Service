using CRMWebService.Infrastructure.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace CRMWebService.Repository
{
    public class SalesReceiptRepository : ISalesReceiptRepository
    {
        public Domain.SalesReceiptLister GetAll(Domain.SalesReceiptLister mLister)
        {
            try
            {
                using (var context = new Persistence.CRMDBEntities())
                {
                    var query = from r in context.Receipts
                                join u in context.Users on r.CollectionAgentId equals u.Id
                                group r by new { u.FirstName, u.LastName } into g
                                select new
                                {
                                    CollectionAgent = g.Key.FirstName + " " + g.Key.LastName,
                                    Total = g.Sum(x => x.TotalPayment),
                                };

                    // Switch to in-memory for complex string filters
                    var efSalesReceipt = query.AsEnumerable();

                    var pagination = mLister.Pagination ?? new Domain.Pagination();

                    pagination.TotalRecord = Math.Max(efSalesReceipt.Count(), 1);

                    if (pagination.Take <= 0)
                    {
                        pagination.Take = pagination.TotalRecord;
                        pagination.Skip = 0;
                    }

                    pagination.CurrentPage = pagination.Take > 0 ? (pagination.Skip / pagination.Take) + 1 : 1;

                    pagination.TotalPage = pagination.Take > 0 ? (int)Math.Ceiling((double)pagination.TotalRecord / pagination.Take) : 1;

                    efSalesReceipt = efSalesReceipt.Skip(pagination.Skip).Take(pagination.Take);

                    mLister.Pagination = pagination;
                    mLister.List = JsonSerializer.Deserialize<List<Domain.SalesReceipt>>(JsonSerializer.Serialize(efSalesReceipt.ToList()));

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
