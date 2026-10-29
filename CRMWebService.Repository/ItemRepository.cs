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
    public class ItemRepository : IItemRepository
    {
        public Domain.ItemLister GetAll(Domain.ItemLister mLister)
        {
            try
            {
                using (var context = new Persistence.CRMDBEntities())
                {
                    var query = from i in context.Items 
                                join at in context.AuditTrails on i.AuditId equals at.Id
                                where at.DeletedBy == null && at.DeletedDate == null
                                select new
                                {
                                    Id = i.Id,
                                    Name = i.Name,
                                    Description = i.Description,
                                    AuditId = i.AuditId,
                                    IsActive = i.IsActive,
                                };
                    // Apply server-side filters where possible
                    if (mLister.SearchCriteria != null)
                    {
                        var criteria = mLister.SearchCriteria;

                        if (criteria.Status.IsNotNullOrEmpty())
                        {
                            if (criteria.Status == "Active")
                                query = query.Where(x => x.IsActive);
                            else if (criteria.Status == "Disabled")
                                query = query.Where(x => !x.IsActive);
                        }

                        if (criteria.Description.IsNotNullOrEmpty())
                            query = query.Where(x => !string.IsNullOrEmpty(x.Description) && x.Description.ToUpper().Contains(criteria.Description.ToUpper()));

                        if (criteria.Name.IsNotNullOrEmpty())
                            query = query.Where(x => !string.IsNullOrEmpty(x.Name) && x.Name.ToUpper().Contains(criteria.Name.ToUpper()));

                    }
                    // Switch to in-memory for complex string filters
                    var efItem = query.AsEnumerable();

                    var pagination = mLister.Pagination ?? new Domain.Pagination();

                    pagination.TotalRecord = Math.Max(efItem.Count(), 1);

                    if (pagination.Take <= 0)
                    {
                        pagination.Take = pagination.TotalRecord;
                        pagination.Skip = 0;
                    }

                    pagination.CurrentPage = pagination.Take > 0 ? (pagination.Skip / pagination.Take) + 1 : 1;

                    pagination.TotalPage = pagination.Take > 0 ? (int)Math.Ceiling((double)pagination.TotalRecord / pagination.Take) : 1;

                    efItem = efItem.Skip(pagination.Skip).Take(pagination.Take);

                    mLister.Pagination = pagination;
                    mLister.List = JsonSerializer.Deserialize<List<Domain.Item>>(JsonSerializer.Serialize(efItem.ToList()));

                }
            }
            catch
            {
                throw;
            }

            return mLister;

        }

        public Domain.Item Upsert(Domain.Item mItem)
        {
            try
            {
                using (var context = new Persistence.CRMDBEntities())
                {
                    var efItem = context.Items.FirstOrDefault(x => x.Id == mItem.Id);
                    if (efItem == null)
                    {
                        var audit = new Persistence.AuditTrail
                        {
                            CreatedBy = mItem.CreatedBy,
                            CreatedDate = DateTime.Now
                        };

                        context.AuditTrails.Add(audit);
                        context.SaveChanges();

                        efItem = new Persistence.Item
                        {
                            AuditId = audit.Id
                        };
                        context.Items.Add(efItem);
                    }
                    else
                    {
                        var audit = context.AuditTrails.Find(efItem.AuditId);
                        if (audit != null)
                        {
                            audit.LastModifiedBy = mItem.LastModifiedBy;
                            audit.LastModifiedDate = DateTime.Now;
                        }
                    }
                    efItem.Name = mItem.Name;
                    efItem.Description = mItem.Description;
                    efItem.IsActive = mItem.IsActive;
                    context.SaveChanges();
                    mItem.Id = efItem.Id;
                }
            }
            catch (Exception)
            {
                throw;
            }

            return mItem;
        }

        public Domain.Item Get(int id)
        {
            var mItem = new Domain.Item();
            try
            {
                using (var context = new Persistence.CRMDBEntities())
                {
                    var efItem = (from i in context.Items
                                    where i.Id == id
                                    select new
                                    {
                                        Id = i.Id,
                                        Name = i.Name,
                                        Description = i.Description,
                                        AuditId = i.AuditId,
                                        IsActive = i.IsActive,
                                    }).FirstOrDefault();

                    if (efItem != null)
                    {
                        mItem = JsonSerializer.Deserialize<Domain.Item>(JsonSerializer.Serialize(efItem));
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return mItem;
        }

        public void Delete(int id, int loginUserId)
        {
            try
            {
                using (var context = new Persistence.CRMDBEntities())
                {
                    var efItem = context.Items.FirstOrDefault(x => x.Id == id);
                    if (efItem != null)
                    {
                        var audit = context.AuditTrails.Find(efItem.AuditId);
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

        public List<Domain.Item> GetActiveItem()
        {
            var mItem = new List<Domain.Item>();
            try
            {
                using (var context = new Persistence.CRMDBEntities())
                {
                    var efItem = (from c in context.Items
                                  where c.IsActive
                                  select new
                                  {
                                      Id = c.Id,
                                      Name = c.Name,
                                  }).AsEnumerable();

                    mItem = JsonSerializer.Deserialize<List<Domain.Item>>(JsonSerializer.Serialize(efItem.ToList()));
                }
            }
            catch (Exception)
            {
                throw;
            }
            return mItem;
        }
    }
}
