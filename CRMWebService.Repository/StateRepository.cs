using CRMWebService.Infrastructure.IRepository;
using CRMWebService.Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace CRMWebService.Repository
{
    public class StateRepository : IStateRepository
    {
        public Domain.StateLister GetAll(Domain.StateLister mLister)
        {
            try
            {
                using (var context = new Persistence.CRMDBEntities())
                {
                    var query = from s in context.States
                                join at in context.AuditTrails on s.AuditId equals at.Id
                                where at.DeletedBy == null && at.DeletedDate == null
                                orderby s.Id descending
                                select new
                                {
                                    Id = s.Id,
                                    Code = s.Code,
                                    Name = s.Name,
                                    IsActive = s.IsActive,
                                };
                    if (mLister.SearchCriteria != null)
                    {
                        var criteria = mLister.SearchCriteria;

                        if (criteria.Code > 0)
                            query = query.Where(x => x.Code == criteria.Code);

                        if (criteria.Name.IsNotNullOrEmpty())
                            query = query.Where(x => !string.IsNullOrEmpty(x.Name) && x.Name.ToUpper().Contains(criteria.Name.ToUpper()));

                        if (criteria.Status.IsNotNullOrEmpty())
                        {
                            if (criteria.Status == "Active")
                                query = query.Where(x => x.IsActive);
                            else if (criteria.Status == "Disabled")
                                query = query.Where(x => !x.IsActive);
                        }
                    }

                    var efState = query.AsEnumerable();

                    var pagination = mLister.Pagination ?? new Domain.Pagination();

                    pagination.TotalRecord = Math.Max(efState.Count(), 1);

                    if (pagination.Take <= 0)
                    {
                        pagination.Take = pagination.TotalRecord;
                        pagination.Skip = 0;
                    }

                    pagination.CurrentPage = pagination.Take > 0 ? (pagination.Skip / pagination.Take) + 1 : 1;

                    pagination.TotalPage = pagination.Take > 0 ? (int)Math.Ceiling((double)pagination.TotalRecord / pagination.Take) : 1;

                    efState = efState.Skip(pagination.Skip).Take(pagination.Take);

                    mLister.Pagination = pagination;
                    mLister.List = JsonSerializer.Deserialize<List<Domain.State>>(JsonSerializer.Serialize(efState.ToList()));
                }
            }
            catch (Exception)
            {
                throw;
            }

            return mLister;
        }

        public Domain.State Upsert(Domain.State mState)
        {
            try
            {
                using (var context = new Persistence.CRMDBEntities())
                {
                    var efState = context.States.FirstOrDefault(x => x.Id == mState.Id);
                    if (efState == null)
                    {
                        var audit = new Persistence.AuditTrail
                        {
                            CreatedBy = mState.CreatedBy,
                            CreatedDate = DateTime.Now
                        };

                        context.AuditTrails.Add(audit);
                        context.SaveChanges();

                        efState = new Persistence.State
                        {
                            AuditId = audit.Id
                        };
                        context.States.Add(efState);
                    }
                    else
                    {
                        var audit = context.AuditTrails.Find(efState.AuditId);
                        if (audit != null)
                        {
                            audit.LastModifiedBy = mState.LastModifiedBy;
                            audit.LastModifiedDate = DateTime.Now;
                        }
                    }

                    efState.Name = mState.Name;
                    efState.Code = mState.Code;
                    efState.IsActive = mState.IsActive;

                    context.SaveChanges();
                    mState.Id = efState.Id;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return mState;
        }

        public Domain.State Get(int id)
        {
            var mState = new Domain.State();
            try
            {
                using (var context = new Persistence.CRMDBEntities())
                {
                    var efState = (from s in context.States
                                   where s.Id == id
                                   select new
                                   {
                                       Id = s.Id,
                                       Name = s.Name,
                                       Code = s.Code,
                                       IsActive = s.IsActive,
                                   }).FirstOrDefault();

                    if (efState != null)
                    {
                        mState = JsonSerializer.Deserialize<Domain.State>(JsonSerializer.Serialize(efState));
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return mState;
        }

        public void Delete(int id, int loginUserId)
        {
            try
            {
                using (var context = new Persistence.CRMDBEntities())
                {
                    var efState = context.States.FirstOrDefault(x => x.Id == id);
                    if (efState != null)
                    {
                        var audit = context.AuditTrails.Find(efState.AuditId);
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

        public List<Domain.State> GetActiveStates()
        {
            var mStates = new List<Domain.State>();
            try
            {
                using (var context = new Persistence.CRMDBEntities())
                {
                    var efState = (from s in context.States
                                   join at in context.AuditTrails on s.AuditId equals at.Id
                                   where at.DeletedDate == null && at.DeletedBy == null && s.IsActive
                                   select new
                                   {
                                       Id = s.Id,
                                       Code = s.Code,
                                       Name = s.Name
                                   }).AsEnumerable();

                    mStates = JsonSerializer.Deserialize<List<Domain.State>>(JsonSerializer.Serialize(efState.ToList()));
                }
            }
            catch (Exception)
            {
                throw;
            }
            return mStates;
        }


    }
}
