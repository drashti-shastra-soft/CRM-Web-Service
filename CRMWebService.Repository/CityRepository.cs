using CRMWebService.Infrastructure.IRepository;
using CRMWebService.Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace CRMWebService.Repository
{
    public class CityRepository : ICityRepository
    {
        public Domain.CityLister GetAll(Domain.CityLister mLister)
        {
            try
            {
                using (var context = new Persistence.CRMDBEntities())
                {
                    var query = from c in context.Cities
                                join s in context.States on c.StateId equals s.Id
                                join atc in context.AuditTrails on c.AuditId equals atc.Id
                                join ats in context.AuditTrails on s.AuditId equals ats.Id
                                where atc.DeletedBy == null && atc.DeletedDate == null &&
                                            ats.DeletedBy == null && ats.DeletedDate == null
                                select new
                                {
                                    Id = c.Id,
                                    StateId = c.StateId,
                                    State = s.Code + " - " + s.Name,
                                    Name = c.Name,
                                    IsActive = c.IsActive,
                                };

                    if (mLister.SearchCriteria != null)
                    {
                        var criteria = mLister.SearchCriteria;

                        if (criteria.State.IsNotNullOrEmpty())
                            query = query.Where(x => !string.IsNullOrEmpty(x.State) && x.State.ToUpper().Contains(criteria.State.ToUpper()));

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

                    var efCity = query.AsEnumerable();

                    var pagination = mLister.Pagination ?? new Domain.Pagination();

                    pagination.TotalRecord = Math.Max(efCity.Count(), 1);

                    if (pagination.Take <= 0)
                    {
                        pagination.Take = pagination.TotalRecord;
                        pagination.Skip = 0;
                    }

                    pagination.CurrentPage = pagination.Take > 0 ? (pagination.Skip / pagination.Take) + 1 : 1;

                    pagination.TotalPage = pagination.Take > 0 ? (int)Math.Ceiling((double)pagination.TotalRecord / pagination.Take) : 1;

                    efCity = efCity.Skip(pagination.Skip).Take(pagination.Take);

                    mLister.Pagination = pagination;
                    mLister.List = JsonSerializer.Deserialize<List<Domain.City>>(JsonSerializer.Serialize(efCity.ToList()));

                }
            }
            catch (Exception)
            {
                throw;
            }
            return mLister;
        }

        public Domain.City Upsert(Domain.City mCity)
        {
            try
            {
                using (var context = new Persistence.CRMDBEntities())
                {
                    var efCity = context.Cities.FirstOrDefault(x => x.Id == mCity.Id);
                    if (efCity == null)
                    {
                        var audit = new Persistence.AuditTrail
                        {
                            CreatedBy = mCity.CreatedBy,
                            CreatedDate = DateTime.Now
                        };

                        context.AuditTrails.Add(audit);
                        context.SaveChanges();

                        efCity = new Persistence.City
                        {
                            AuditId = audit.Id
                        };
                        context.Cities.Add(efCity);
                    }
                    else
                    {
                        var audit = context.AuditTrails.Find(efCity.AuditId);
                        if (audit != null)
                        {
                            audit.LastModifiedBy = mCity.LastModifiedBy;
                            audit.LastModifiedDate = DateTime.Now;
                        }
                    }

                    efCity.StateId = mCity.StateId;
                    efCity.Name = mCity.Name;
                    efCity.IsActive = mCity.IsActive;

                    context.SaveChanges();
                    mCity.Id = efCity.Id;

                }
            }
            catch (Exception)
            {
                throw;
            }
            return mCity;
        }

        public Domain.City Get(int id)
        {
            var mCity = new Domain.City();
            try
            {
                using (var context = new Persistence.CRMDBEntities())
                {
                    var efCity = (from c in context.Cities
                                  where c.Id == id
                                  select new
                                  {
                                      Id = c.Id,
                                      Name = c.Name,
                                      StateId = c.StateId,
                                      IsActive = c.IsActive,
                                  }).FirstOrDefault();

                    if (efCity != null)
                    {
                        mCity = JsonSerializer.Deserialize<Domain.City>(JsonSerializer.Serialize(efCity));
                    }


                }
            }
            catch (Exception)
            {
                throw;
            }
            return mCity;
        }

        public void Delete(int id, int loginUserId)
        {
            try
            {
                using (var context = new Persistence.CRMDBEntities())
                {
                    var efCity = context.Cities.FirstOrDefault(x => x.Id == id);
                    if (efCity != null)
                    {
                        var audit = context.AuditTrails.Find(efCity.AuditId);
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

        public List<Domain.City> GetActiveBy(int stateId)
        {
            var mCities = new List<Domain.City>();
            try
            {
                using (var context = new Persistence.CRMDBEntities())
                {
                    var efCities = (from c in context.Cities
                                    join at in context.AuditTrails on c.AuditId equals at.Id
                                    where at.DeletedDate == null && at.DeletedBy == null && c.IsActive && c.StateId == stateId
                                    select new
                                    {
                                        Id = c.Id,
                                        Name = c.Name
                                    }).AsEnumerable();

                    mCities = JsonSerializer.Deserialize<List<Domain.City>>(JsonSerializer.Serialize(efCities.ToList()));
                }
            }
            catch (Exception)
            {
                throw;
            }
            return mCities;
        }

        public List<Domain.City> GetActiveBy(string state)
        {
            var mCities = new List<Domain.City>();
            try
            {
                using (var context = new Persistence.CRMDBEntities())
                {
                    var efCities = (from c in context.Cities
                                    join s in context.States on c.StateId equals s.Id
                                    join at in context.AuditTrails on c.AuditId equals at.Id
                                    where at.DeletedDate == null && at.DeletedBy == null && c.IsActive
                                    select new
                                    {
                                        Id = c.Id,
                                        Name = c.Name,
                                        State = s.Name
                                    }).AsEnumerable();

                    efCities = efCities.Where(x => !string.IsNullOrEmpty(x.State) && !string.IsNullOrWhiteSpace(x.State) && x.State.Trim().ToUpper() == state.Trim().ToUpper());

                    mCities = JsonSerializer.Deserialize<List<Domain.City>>(JsonSerializer.Serialize(efCities.ToList()));
                }
            }
            catch (Exception)
            {
                throw;
            }
            return mCities;
        }

    }
}
