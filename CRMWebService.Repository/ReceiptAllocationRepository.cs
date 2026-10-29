using CRMWebService.Infrastructure.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace CRMWebService.Repository
{
    public class ReceiptAllocationRepository : IReceiptAllocationRepository
    {
        public Domain.ReceiptAllocationLister GetAll(Domain.ReceiptAllocationLister mLister)
        {
            try
            {
                using (var context = new Persistence.CRMDBEntities())
                {
                    var query = from ra in context.ReceiptAllocations
                                join at in context.AuditTrails on ra.AuditId equals at.Id
                                where at.DeletedBy == null && at.DeletedDate == null
                                select new
                                {
                                    Id = ra.Id,
                                    ReceiptId = ra.ReceiptId,
                                    InvoiceId = ra.InvoiceId,
                                    Amount = ra.Amount,
                                };

                    // Switch to in-memory for complex string filters
                    var efReceiptAllocation = query.AsEnumerable();

                    var pagination = mLister.Pagination ?? new Domain.Pagination();

                    pagination.TotalRecord = Math.Max(efReceiptAllocation.Count(), 1);

                    if (pagination.Take <= 0)
                    {
                        pagination.Take = pagination.TotalRecord;
                        pagination.Skip = 0;
                    }

                    pagination.CurrentPage = pagination.Take > 0 ? (pagination.Skip / pagination.Take) + 1 : 1;

                    pagination.TotalPage = pagination.Take > 0 ? (int)Math.Ceiling((double)pagination.TotalRecord / pagination.Take) : 1;

                    efReceiptAllocation = efReceiptAllocation.Skip(pagination.Skip).Take(pagination.Take);

                    mLister.Pagination = pagination;

                    mLister.List = JsonSerializer.Deserialize<List<Domain.ReceiptAllocation>>(JsonSerializer.Serialize(efReceiptAllocation.ToList()));
                }
            }
            catch (Exception)
            {
                throw;
            }
            return mLister;
        }

        public Domain.ReceiptAllocation Upsert(Domain.ReceiptAllocation mReceiptAllocation)
        {
            try
            {
                using (var context = new Persistence.CRMDBEntities())
                {
                    var efReceiptAllocation = context.ReceiptAllocations.FirstOrDefault(x => x.Id == mReceiptAllocation.Id);
                    if (efReceiptAllocation == null)
                    {
                        var audit = new Persistence.AuditTrail
                        {
                            CreatedBy = mReceiptAllocation.CreatedBy,
                            CreatedDate = DateTime.Now
                        };

                        context.AuditTrails.Add(audit);
                        context.SaveChanges();

                        efReceiptAllocation = new Persistence.ReceiptAllocation
                        {
                            AuditId = audit.Id
                        };
                        context.ReceiptAllocations.Add(efReceiptAllocation);
                    }
                    else
                    {
                        var audit = context.AuditTrails.Find(efReceiptAllocation.AuditId);
                        if (audit != null)
                        {
                            audit.LastModifiedBy = mReceiptAllocation.LastModifiedBy;
                            audit.LastModifiedDate = DateTime.Now;
                        }
                    }

                    efReceiptAllocation.ReceiptId = mReceiptAllocation.ReceiptId;
                    efReceiptAllocation.InvoiceId = mReceiptAllocation.InvoiceId;
                    efReceiptAllocation.Amount = mReceiptAllocation.Amount;
                    context.SaveChanges();
                    mReceiptAllocation.Id = efReceiptAllocation.Id;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return mReceiptAllocation;
        }

        public List<Domain.ReceiptAllocation> Get(int id)
        {
            var mReceiptAllocation = new List<Domain.ReceiptAllocation>();
            try
            {
                using (var context = new Persistence.CRMDBEntities())
                {
                    var efReceiptAllocation = (from ra in context.ReceiptAllocations
                                               join i in context.Invoices on ra.InvoiceId equals i.Id
                                               where ra.ReceiptId == id
                                               select new
                                               {
                                                   Id = ra.Id,
                                                   ReceiptId = ra.ReceiptId,
                                                   InvoiceId = ra.InvoiceId,
                                                   InvoiceNumber = i.InvoiceNumber,
                                                   NetAmount = i.NetAmount,
                                                   TotalPayment = i.TotalPayment,
                                                   Status = i.Status,
                                                   Amount = ra.Amount,
                                               }).AsEnumerable();

                    if (efReceiptAllocation != null)
                    {
                        mReceiptAllocation = JsonSerializer.Deserialize<List<Domain.ReceiptAllocation>>(JsonSerializer.Serialize(efReceiptAllocation.ToList()));
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return mReceiptAllocation;
        }

        public void Delete(int id, int loginUserId)
        {
            try
            {
                using (var context = new Persistence.CRMDBEntities())
                {
                    var efReceiptAllocation = context.ReceiptAllocations.FirstOrDefault(x => x.Id == id);
                    if (efReceiptAllocation != null)
                    {
                        var audit = context.AuditTrails.Find(efReceiptAllocation.AuditId);
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
    }
}
