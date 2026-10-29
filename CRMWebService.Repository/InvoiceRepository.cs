using CRMWebService.Infrastructure.IRepository;
using CRMWebService.Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json;

namespace CRMWebService.Repository
{
    public class InvoiceRepository : IInvoiceRepository
    {
        public Domain.InvoiceLister GetAll(Domain.InvoiceLister mLister)
        {
            try
            {
                using (var context = new Persistence.CRMDBEntities())
                {
                    var query = from n in context.Invoices
                                join at in context.AuditTrails on n.AuditId equals at.Id
                                where at.DeletedBy == null && at.DeletedDate == null
                                select new
                                {
                                    Id = n.Id,
                                    Date = n.Date,
                                    BookType = n.BookType,
                                    ClientId = n.ClientId,
                                    ClientName = n.ClientName,
                                    InvoiceNumber = n.InvoiceNumber,
                                    ChallanNumber = n.ChallanNumber,
                                    LRNumber = n.LRNumber,
                                    TaxableValue = n.TaxableValue,
                                    SGST = n.SGST,
                                    CGST = n.CGST,
                                    IGST = n.IGST,
                                    NetAmount = n.NetAmount,
                                    DyeingSupervisorId = n.DyeingSupervisorId,
                                    DyeingSupervisorName = n.DyeingSupervisorName,
                                    CollectionAgentId = n.CollectionAgentId,
                                    CollectionAgentName = n.CollectionAgentName,
                                    BrokerId = n.BrokerId,
                                    BrokerName = n.BrokerName,
                                    VehicleNumber = n.VehicleNumber,
                                    AuditId = n.AuditId,
                                };
                    // Apply server-side filters where possible
                    if (mLister.SearchCriteria != null)
                    {
                        var criteria = mLister.SearchCriteria;

                        if (criteria.ClientName.IsNotNullOrEmpty())
                            query = query.Where(x => !string.IsNullOrEmpty(x.ClientName) && x.ClientName.ToUpper().Contains(criteria.ClientName.ToUpper()));

                        if (criteria.InvoiceNumber.IsNotNullOrEmpty())
                            query = query.Where(x => !string.IsNullOrEmpty(x.InvoiceNumber) && x.InvoiceNumber.ToUpper().Contains(criteria.InvoiceNumber.ToUpper()));

                        if (criteria.ChallanNumber.IsNotNullOrEmpty())
                            query = query.Where(x => !string.IsNullOrEmpty(x.ChallanNumber) && x.ChallanNumber.ToUpper().Contains(criteria.ChallanNumber.ToUpper()));

                        if (criteria.LRNumber.IsNotNullOrEmpty())
                            query = query.Where(x => !string.IsNullOrEmpty(x.LRNumber) && x.LRNumber.ToUpper().Contains(criteria.LRNumber.ToUpper()));

                        if (criteria.VehicleNumber.IsNotNullOrEmpty())
                            query = query.Where(x => !string.IsNullOrEmpty(x.VehicleNumber) && x.VehicleNumber.ToUpper().Contains(criteria.VehicleNumber.ToUpper()));

                        if (criteria.DyeingSupervisorName.IsNotNullOrEmpty())
                            query = query.Where(x => !string.IsNullOrEmpty(x.DyeingSupervisorName) && x.DyeingSupervisorName.ToUpper().Contains(criteria.DyeingSupervisorName.ToUpper()));

                        if (criteria.CollectionAgentName.IsNotNullOrEmpty())
                            query = query.Where(x => !string.IsNullOrEmpty(x.CollectionAgentName) && x.CollectionAgentName.ToUpper().Contains(criteria.CollectionAgentName.ToUpper()));

                        if (criteria.BrokerName.IsNotNullOrEmpty())
                            query = query.Where(x => !string.IsNullOrEmpty(x.BrokerName) && x.BrokerName.ToUpper().Contains(criteria.BrokerName.ToUpper()));

                    }
                    // Switch to in-memory for complex string filters
                    var efInvoice = query.AsEnumerable();

                    var pagination = mLister.Pagination ?? new Domain.Pagination();

                    pagination.TotalRecord = Math.Max(efInvoice.Count(), 1);

                    if (pagination.Take <= 0)
                    {
                        pagination.Take = pagination.TotalRecord;
                        pagination.Skip = 0;
                    }

                    pagination.CurrentPage = pagination.Take > 0 ? (pagination.Skip / pagination.Take) + 1 : 1;

                    pagination.TotalPage = pagination.Take > 0 ? (int)Math.Ceiling((double)pagination.TotalRecord / pagination.Take) : 1;

                    efInvoice = efInvoice.Skip(pagination.Skip).Take(pagination.Take);

                    mLister.Pagination = pagination;
                    mLister.List = JsonSerializer.Deserialize<List<Domain.Invoice>>(JsonSerializer.Serialize(efInvoice.ToList()));

                }
            }
            catch
            {
                throw;
            }

            return mLister;

        }

        public Domain.Invoice Upsert(Domain.Invoice mInvoice)
        {
            try
            {
                using (var context = new Persistence.CRMDBEntities())
                {
                    var efInvoice = context.Invoices.FirstOrDefault(x => x.Id == mInvoice.Id);
                    if (efInvoice == null)
                    {
                        var audit = new Persistence.AuditTrail
                        {
                            CreatedBy = mInvoice.CreatedBy,
                            CreatedDate = DateTime.Now
                        };

                        context.AuditTrails.Add(audit);
                        context.SaveChanges();

                        efInvoice = new Persistence.Invoice
                        {
                            AuditId = audit.Id,
                            Status = Infrastructure.Utils.Enums.InvoiceStatus.Pending.ToString()
                        };
                        context.Invoices.Add(efInvoice);
                    }
                    else
                    {
                        var audit = context.AuditTrails.Find(efInvoice.AuditId);
                        if (audit != null)
                        {
                            audit.LastModifiedBy = mInvoice.LastModifiedBy;
                            audit.LastModifiedDate = DateTime.Now;
                        }
                    }
                    efInvoice.Date = mInvoice.Date;
                    efInvoice.BookType = mInvoice.BookType;
                    efInvoice.ClientId = mInvoice.ClientId;
                    efInvoice.ClientName = mInvoice.ClientName;
                    efInvoice.InvoiceNumber = mInvoice.InvoiceNumber;
                    efInvoice.ChallanNumber = mInvoice.ChallanNumber;
                    efInvoice.LRNumber = mInvoice.LRNumber;
                    efInvoice.VehicleNumber = mInvoice.VehicleNumber;
                    efInvoice.DyeingSupervisorId = mInvoice.DyeingSupervisorId;
                    efInvoice.DyeingSupervisorName = mInvoice.DyeingSupervisorName;
                    efInvoice.CollectionAgentId = mInvoice.CollectionAgentId;
                    efInvoice.CollectionAgentName = mInvoice.CollectionAgentName;
                    efInvoice.BrokerId = mInvoice.BrokerId;
                    efInvoice.BrokerName = mInvoice.BrokerName;
                    efInvoice.TaxableValue = mInvoice.TaxableValue;
                    efInvoice.SGST = mInvoice.SGST;
                    efInvoice.CGST = mInvoice.CGST;
                    efInvoice.IGST = mInvoice.IGST;
                    efInvoice.NetAmount = mInvoice.NetAmount;                    
                    efInvoice.TotalPayment = mInvoice.TotalPayment;
                    context.SaveChanges();
                    mInvoice.Id = efInvoice.Id;
                }
            }
            catch (Exception)
            {
                throw;
            }

            return mInvoice;
        }

        public Domain.Invoice Get(int id)
        {
            var mInvoice = new Domain.Invoice();
            try
            {
                using (var context = new Persistence.CRMDBEntities())
                {
                    var efInvoice = (from n in context.Invoices
                                     where n.Id == id
                                     select new
                                     {
                                         Id = n.Id,
                                         BookType = n.BookType,
                                         ClientId = n.ClientId,
                                         ClientName = n.ClientName,
                                         InvoiceNumber = n.InvoiceNumber,
                                         ChallanNumber = n.ChallanNumber,
                                         LRNumber = n.LRNumber,
                                         TaxableValue = n.TaxableValue,
                                         SGST = n.SGST,
                                         CGST = n.CGST,
                                         IGST = n.IGST,
                                         NetAmount = n.NetAmount,
                                         DyeingSupervisorId = n.DyeingSupervisorId,
                                         DyeingSupervisorName = n.DyeingSupervisorName,
                                         CollectionAgentId = n.CollectionAgentId,
                                         CollectionAgentName = n.CollectionAgentName,
                                         VehicleNumber = n.VehicleNumber,
                                         BrokerName = n.BrokerName,
                                         AuditId = n.AuditId,
                                         BrokerId = n.BrokerId,
                                         Date = n.Date,
                                         TotalPayment = n.TotalPayment,
                                     }).FirstOrDefault();

                    if (efInvoice != null)
                    {
                        mInvoice = JsonSerializer.Deserialize<Domain.Invoice>(JsonSerializer.Serialize(efInvoice));
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return mInvoice;
        }

        public List<Domain.Invoice> GetBy(int clientId)
        {
            var mInvoices = new List<Domain.Invoice>();
            try
            {
                using (var context = new Persistence.CRMDBEntities())
                {
                    var efInvoices = (from i in context.Invoices
                                      join at in context.AuditTrails on i.AuditId equals at.Id
                                      where i.ClientId == clientId && at.DeletedDate == null && at.DeletedBy == null && (i.Status == "Pending" || i.Status == "PartiallyPaid")
                                      select new
                                      {
                                          Id = i.Id,
                                          Date = i.Date,
                                          InvoiceNumber = i.InvoiceNumber,                                          
                                          NetAmount = i.NetAmount,
                                          TotalPayment = i.TotalPayment
                                      }).AsEnumerable();

                    mInvoices = JsonSerializer.Deserialize<List<Domain.Invoice>>(JsonSerializer.Serialize(efInvoices.ToList()));
                }
            }
            catch (Exception)
            {
                throw;
            }

            return mInvoices;
        }

        public void Delete(int id, int loginUserId)
        {
            try
            {
                using (var context = new Persistence.CRMDBEntities())
                {
                    var efClient = context.Invoices.FirstOrDefault(x => x.Id == id);
                    if (efClient != null)
                    {
                        var audit = context.AuditTrails.Find(efClient.AuditId);
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

        public void UpdatePayment(Domain.Invoice mInvoice)
        {
            try
            {
                using (var context = new Persistence.CRMDBEntities())
                {
                    var efInvoice = context.Invoices.FirstOrDefault(x => x.Id == mInvoice.Id);
                    if (efInvoice != null && efInvoice.Id > 0)
                    {
                        efInvoice.TotalPayment += mInvoice.TotalPayment;
                        efInvoice.Status = mInvoice.Status;                        
                        var audit = context.AuditTrails.Find(efInvoice.AuditId);
                        if (audit != null)
                        {
                            audit.LastModifiedBy = mInvoice.LastModifiedBy;
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
