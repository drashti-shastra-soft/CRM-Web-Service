using CRMWebService.Infrastructure.IRepository;
using CRMWebService.Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text.Json;

namespace CRMWebService.Repository
{
    public class ReceiptRepository : IReceiptRepository
    {
        public Domain.ReceiptLister GetAll(Domain.ReceiptLister mLister)
        {
            try
            {
                using (var context = new Persistence.CRMDBEntities())
                {
                    var query = from r in context.Receipts
                                join c in context.Clients on r.ClientId equals c.Id
                                join ca in context.Clients on r.CollectionAgentId equals ca.Id
                                join at in context.AuditTrails on r.AuditId equals at.Id
                                where at.DeletedBy == null && at.DeletedDate == null
                                orderby r.Id descending
                                select new
                                {
                                    Id = r.Id,
                                    VoucherNumber = r.VoucherNumber,
                                    ClientId = r.ClientId,
                                    CollectionAgentId = r.CollectionAgentId,
                                    CollectionAgent = ca.Name,
                                    Client = c.Name,
                                    VoucherDate = r.VoucherDate,
                                    Method = r.Method,
                                    TotalPayment = r.TotalPayment,
                                    CashDiscount = r.CashDiscount,
                                    TDS = r.TDS,
                                    RateDifference = r.RateDifference,
                                    ClaimAmount = r.ClaimAmount,
                                    FinalAmount = r.FinalAmount,
                                    BankId = r.BankId,
                                    ReferenceNumber = r.ReferenceNumber,
                                    Status = r.Status,
                                };

                    if (mLister.SearchCriteria != null)
                    {
                        var criteria = mLister.SearchCriteria;

                        if (criteria.VoucherNumber > 0)
                            query = query.Where(x => x.VoucherNumber == criteria.VoucherNumber);

                        if (criteria.ClientId > 0)
                            query = query.Where(x => x.ClientId == criteria.ClientId);

                        if (criteria.CollectionAgentId > 0)
                            query = query.Where(x => x.CollectionAgentId == criteria.CollectionAgentId);
                        
                    }

                    // Switch to in-memory for complex string filters
                    var efReceipt = query.AsEnumerable();

                    var pagination = mLister.Pagination ?? new Domain.Pagination();

                    pagination.TotalRecord = Math.Max(efReceipt.Count(), 1);

                    if (pagination.Take <= 0)
                    {
                        pagination.Take = pagination.TotalRecord;
                        pagination.Skip = 0;
                    }

                    pagination.CurrentPage = pagination.Take > 0 ? (pagination.Skip / pagination.Take) + 1 : 1;

                    pagination.TotalPage = pagination.Take > 0 ? (int)Math.Ceiling((double)pagination.TotalRecord / pagination.Take) : 1;

                    efReceipt = efReceipt.Skip(pagination.Skip).Take(pagination.Take);

                    mLister.Pagination = pagination;

                    mLister.List = JsonSerializer.Deserialize<List<Domain.Receipt>>(JsonSerializer.Serialize(efReceipt.ToList()));
                }
            }
            catch (Exception)
            {
                throw;
            }
            return mLister;
        }

        public Domain.Receipt Upsert(Domain.Receipt mReceipt)
        {
            try
            {
                using (var context = new Persistence.CRMDBEntities())
                {
                    var efReceipt = context.Receipts.FirstOrDefault(x => x.Id == mReceipt.Id);
                    if (efReceipt == null)
                    {
                        var audit = new Persistence.AuditTrail
                        {
                            CreatedBy = mReceipt.CreatedBy,
                            CreatedDate = DateTime.Now
                        };

                        context.AuditTrails.Add(audit);
                        context.SaveChanges();

                        efReceipt = new Persistence.Receipt
                        {
                            AuditId = audit.Id
                        };
                        context.Receipts.Add(efReceipt);
                    }
                    else
                    {
                        var audit = context.AuditTrails.Find(efReceipt.AuditId);
                        if (audit != null)
                        {
                            audit.LastModifiedBy = mReceipt.LastModifiedBy;
                            audit.LastModifiedDate = DateTime.Now;
                        }
                    }

                    efReceipt.VoucherNumber = mReceipt.VoucherNumber;
                    efReceipt.CollectionAgentId = mReceipt.CollectionAgentId;
                    efReceipt.ClientId = mReceipt.ClientId;
                    efReceipt.VoucherDate = mReceipt.VoucherDate;
                    efReceipt.Method = mReceipt.Method;
                    efReceipt.TotalPayment = mReceipt.TotalPayment;
                    efReceipt.CashDiscount = mReceipt.CashDiscount;
                    efReceipt.TDS = mReceipt.TDS;
                    efReceipt.RateDifference = mReceipt.RateDifference;
                    efReceipt.ClaimAmount = mReceipt.ClaimAmount;
                    efReceipt.FinalAmount = mReceipt.FinalAmount;
                    efReceipt.BankId = mReceipt.BankId;
                    efReceipt.ReferenceNumber = mReceipt.ReferenceNumber;
                    efReceipt.Status = mReceipt.Status;
                    efReceipt.ChequeDate = mReceipt.ChequeDate;

                    context.SaveChanges();
                    mReceipt.Id = efReceipt.Id;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return mReceipt;
        }

        public Domain.Receipt Get(int id)
        {
            var mReceipt = new Domain.Receipt();
            try
            {
                using (var context = new Persistence.CRMDBEntities())
                {
                    var efReceipt = (from r in context.Receipts
                                     where r.Id == id
                                     select new
                                     {
                                         Id = r.Id,
                                         VoucherNumber = r.VoucherNumber,
                                         CollectionAgentId = r.CollectionAgentId,
                                         ClientId = r.ClientId,
                                         VoucherDate = r.VoucherDate,
                                         Method = r.Method,
                                         TotalPayment = r.TotalPayment,
                                         CashDiscount = r.CashDiscount,
                                         TDS = r.TDS,
                                         RateDifference = r.RateDifference,
                                         ClaimAmount = r.ClaimAmount,
                                         FinalAmount = r.FinalAmount,
                                         BankId = r.BankId,
                                         ReferenceNumber = r.ReferenceNumber,
                                         Status = r.Status,
                                         ChequeDate = r.ChequeDate,
                                     }).FirstOrDefault();

                    if (efReceipt != null)
                    {
                        mReceipt = JsonSerializer.Deserialize<Domain.Receipt>(JsonSerializer.Serialize(efReceipt));
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return mReceipt;
        }

        public void Delete(int id, int loginUserId)
        {
            try
            {
                using (var context = new Persistence.CRMDBEntities())
                {
                    var efReceipt = context.Receipts.FirstOrDefault(x => x.Id == id);
                    if (efReceipt != null)
                    {
                        var audit = context.AuditTrails.Find(efReceipt.AuditId);
                        if (audit != null)
                        {
                            audit.DeletedBy = loginUserId;
                            audit.DeletedDate = DateTime.Now;
                        }
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void PatchReceiptStatus(int id, string status, int loginUserId)
        {
            try
            {
                using (var context = new Persistence.CRMDBEntities())
                {
                    var efReceipt = context.Receipts.FirstOrDefault(x => x.Id == id);
                    if (efReceipt != null)
                    {
                        efReceipt.Status = status;
                        var audit = context.AuditTrails.Find(efReceipt.AuditId);
                        if (audit != null)
                        {
                            audit.LastModifiedBy = loginUserId;
                            audit.LastModifiedDate = DateTime.Now;
                        }
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
