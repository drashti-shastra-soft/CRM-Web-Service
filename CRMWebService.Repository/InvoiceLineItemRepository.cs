using CRMWebService.Infrastructure.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text.Json;

namespace CRMWebService.Repository
{
    public class InvoiceLineItemRepository : IInvoiceLineItemRepository
    {
        public void Create(Domain.InvoiceLineItem mInvoiceLineItem)
        {
            try
            {
                using (var context = new Persistence.CRMDBEntities())
                {
                    var audit = new Persistence.AuditTrail
                    {
                        CreatedBy = mInvoiceLineItem.CreatedBy,
                        CreatedDate = DateTime.Now
                    };

                    context.AuditTrails.Add(audit);
                    context.SaveChanges();

                    var efInvoiceLineItem = new Persistence.InvoiceLineItem
                    {
                        InvoiceId = mInvoiceLineItem.InvoiceId,
                        ItemId = mInvoiceLineItem.ItemId,
                        ItemName = mInvoiceLineItem.ItemName,
                        Style = mInvoiceLineItem.Style,
                        LotNumber = mInvoiceLineItem.LotNumber,
                        GreyMeter = mInvoiceLineItem.GreyMeter,
                        Piece = mInvoiceLineItem.Piece,
                        PartPiece = mInvoiceLineItem.PartPiece,
                        GreyValue = mInvoiceLineItem.GreyValue,
                        FinishMeter = mInvoiceLineItem.FinishMeter,
                        Shortage = mInvoiceLineItem.Shortage,
                        Rate = mInvoiceLineItem.Rate,
                        Amount = mInvoiceLineItem.Amount,
                        AddToTotal = mInvoiceLineItem.AddToTotal,
                        SubtractFromTotal = mInvoiceLineItem.SubtractFromTotal,
                        AuditId = audit.Id
                    };

                    context.InvoiceLineItems.Add(efInvoiceLineItem);
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Domain.InvoiceLineItem> Get(int id)
        {
            var mInvoiceLineItem = new List<Domain.InvoiceLineItem>();
            try
            {
                using (var context = new Persistence.CRMDBEntities())
                {
                    var efInvoiceLineItem = (from li in context.InvoiceLineItems
                                             where li.InvoiceId == id
                                             select new
                                             {
                                                 Id = li.Id,
                                                 InvoiceId = li.InvoiceId,
                                                 ItemId = li.ItemId,
                                                 ItemName = li.ItemName,
                                                 Style = li.Style,
                                                 LotNumber = li.LotNumber,
                                                 GreyMeter = li.GreyMeter,
                                                 Piece = li.Piece,
                                                 PartPiece = li.PartPiece,
                                                 GreyValue = li.GreyValue,
                                                 FinishMeter = li.FinishMeter,
                                                 Shortage = li.Shortage,
                                                 Rate = li.Rate,
                                                 Amount = li.Amount,
                                                 AddToTotal = li.AddToTotal,
                                                 SubtractFromTotal = li.SubtractFromTotal,
                                             }).AsEnumerable();

                    if (efInvoiceLineItem != null)
                    {
                        mInvoiceLineItem = JsonSerializer.Deserialize<List<Domain.InvoiceLineItem>>(JsonSerializer.Serialize(efInvoiceLineItem.ToList()));
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return mInvoiceLineItem;
        }

        public void Delete(int id, int loginUserId)
        {
            try
            {
                using (var context = new Persistence.CRMDBEntities())
                {
                    var efIvoiceLineItem = context.InvoiceLineItems.FirstOrDefault(x => x.Id == id);
                    if (efIvoiceLineItem != null)
                    {
                        var audit = context.AuditTrails.Find(efIvoiceLineItem.AuditId);
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

        public void DeleteByInvoiceId(int invoiceId, int loginUserId)
        {
            try
            {
                using (var context = new Persistence.CRMDBEntities())
                {
                    var efLineItems = context.InvoiceLineItems.Where(x => x.InvoiceId == invoiceId).ToList();
                    if (efLineItems != null && efLineItems.Count > default(int))
                    {
                        foreach (var efLineItem in efLineItems)
                        {
                            var audit = context.AuditTrails.Find(efLineItem.AuditId);
                            if (audit != null)
                            {
                                audit.DeletedBy = loginUserId;
                                audit.DeletedDate = DateTime.Now;
                            }
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
