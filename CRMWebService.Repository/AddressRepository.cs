using CRMWebService.Infrastructure.IRepository;
using CRMWebService.Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CRMWebService.Repository
{
    public class AddressRepository : IAddressRepository
    {
        public Domain.AddressLister GetAll(Domain.AddressLister mLister)
        {
            try
            {
                using (var context = new Persistence.CRMDBEntities())
                {
                    var query = from c in context.Addresses
                                join at in context.AuditTrails on c.AuditId equals at.Id
                                where at.DeletedBy == null && at.DeletedDate == null
                                select new
                                {
                                    Id = c.Id,
                                    MarketId = c.MarketId,
                                    Line1 = c.Line1,
                                    Line2 = c.Line2,
                                    Line3 = c.Line3,
                                    City = c.City,
                                    State = c.State,
                                    ZipCode = c.ZipCode,
                                };

                    if (mLister.SearchCriteria != null)
                    {
                        var criteria = mLister.SearchCriteria;

                        if (criteria.Line1.IsNotNullOrEmpty())
                            query = query.Where(x => !string.IsNullOrEmpty(x.Line1) && x.Line1.ToUpper().Contains(criteria.Line1.ToUpper()));

                     
                    }

                    var efAddress = query.AsEnumerable();

                    var pagination = mLister.Pagination ?? new Domain.Pagination();

                    pagination.TotalRecord = Math.Max(efAddress.Count(), 1);

                    if (pagination.Take <= 0)
                    {
                        pagination.Take = pagination.TotalRecord;
                        pagination.Skip = 0;
                    }

                    pagination.CurrentPage = pagination.Take > 0 ? (pagination.Skip / pagination.Take) + 1 : 1;

                    pagination.TotalPage = pagination.Take > 0 ? (int)Math.Ceiling((double)pagination.TotalRecord / pagination.Take) : 1;

                    efAddress = efAddress.Skip(pagination.Skip).Take(pagination.Take);

                    mLister.Pagination = pagination;
                    mLister.List = JsonSerializer.Deserialize<List<Domain.Address>>(JsonSerializer.Serialize(efAddress.ToList()));

                }
            }
            catch (Exception)
            {
                throw;
            }
            return mLister;
        }

        public Domain.Address Upsert(Domain.Address mAddress)
        {
            try
            {
                using (var context = new Persistence.CRMDBEntities())
                {
                    var efAddress = context.Addresses.FirstOrDefault(x => x.Id == mAddress.Id);
                    if (efAddress == null)
                    {
                        var audit = new Persistence.AuditTrail
                        {
                            CreatedBy = mAddress.CreatedBy,
                            CreatedDate = DateTime.Now
                        };

                        context.AuditTrails.Add(audit);
                        context.SaveChanges();

                        efAddress = new Persistence.Address
                        {
                            AuditId = audit.Id
                        };
                        context.Addresses.Add(efAddress);
                    }
                    else
                    {
                        var audit = context.AuditTrails.Find(efAddress.AuditId);
                        if (audit != null)
                        {
                            audit.LastModifiedBy = mAddress.LastModifiedBy;
                            audit.LastModifiedDate = DateTime.Now;
                        }
                    }

                    efAddress.MarketId = mAddress.MarketId;
                    efAddress.Line1 = mAddress.Line1;
                    efAddress.Line2 = mAddress.Line2;
                    efAddress.Line3 = mAddress.Line3;
                    efAddress.City = mAddress.City;
                    efAddress.State = mAddress.State;
                    efAddress.ZipCode = mAddress.ZipCode;

                    context.SaveChanges();
                    mAddress.Id = efAddress.Id;

                }
            }
            catch (Exception)
            {
                throw;
            }
            return mAddress;
        }

        public Domain.Address Get(int id)
        {
            var mAddress = new Domain.Address();
            try
            {
                using (var context = new Persistence.CRMDBEntities())
                {
                    var efAddress = (from c in context.Addresses
                                     where c.Id == id
                                     select new
                                     {
                                         Id = c.Id,
                                         MarketId = c.MarketId,
                                         Line1 = c.Line1,
                                         Line2 = c.Line2,
                                         Line3 = c.Line3,
                                         City = c.City,
                                         State = c.State,
                                         ZipCode = c.ZipCode,
                                     }).FirstOrDefault();

                    if (efAddress != null)
                    {
                        mAddress = JsonSerializer.Deserialize<Domain.Address>(JsonSerializer.Serialize(efAddress));
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return mAddress;
        }

        public void Delete(int id, int loginUserId)
        {
            try
            {
                using (var context = new Persistence.CRMDBEntities())
                {
                    var efAddress = context.Addresses.FirstOrDefault(x => x.Id == id);
                    if (efAddress != null)
                    {
                        var audit = context.AuditTrails.Find(efAddress.AuditId);
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
