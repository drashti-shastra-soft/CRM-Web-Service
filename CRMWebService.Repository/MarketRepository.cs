using CRMWebService.Infrastructure.IRepository;
using CRMWebService.Infrastructure.Utils;
using System.Collections.Generic;
using System.Text.Json;
using System.Linq;
using System;

namespace CRMWebService.Repository
{
    public class MarketRepository : IMarketRepository
    {
        public Domain.MarketLister GetAll(Domain.MarketLister mLister)
        {
            try
            {
                using (var context = new Persistence.CRMDBEntities())
                {
                    var query = from m in context.Markets
                                join at in context.AuditTrails on m.AuditId equals at.Id
                                where at.DeletedBy == null && at.DeletedDate == null
                                select new
                                {
                                    Id = m.Id,
                                    Name = m.Name,
                                    Line1 = m.Line1,
                                    Line2 = m.Line2,
                                    City = m.City,
                                    State = m.State,
                                    ZipCode = m.ZipCode,
                                    Rank = m.Rank,
                                };


                    if (mLister.SearchCriteria != null)
                    {
                        var criteria = mLister.SearchCriteria;

                        if (criteria.Name.IsNotNullOrEmpty())
                            query = query.Where(x => !string.IsNullOrEmpty(x.Name) && x.Name.ToUpper().Contains(criteria.Name.ToUpper()));

                        if (criteria.Rank > 0)
                            query = query.Where(x => x.Rank == criteria.Rank);

                        if (criteria.ZipCode.IsNotNullOrEmpty())
                            query = query.Where(x => !string.IsNullOrEmpty(x.ZipCode) && x.ZipCode.ToUpper().Contains(criteria.ZipCode.ToUpper()));

                        if (criteria.Line1.IsNotNullOrEmpty())
                            query = query.Where(x => !string.IsNullOrEmpty(x.Line1) && x.Line1.ToUpper().Contains(criteria.Line1.ToUpper()));

                        if (criteria.Line2.IsNotNullOrEmpty())
                            query = query.Where(x => !string.IsNullOrEmpty(x.Line2) && x.Line2.ToUpper().Contains(criteria.Line2.ToUpper()));

                        if (criteria.City.IsNotNullOrEmpty())
                            query = query.Where(x => !string.IsNullOrEmpty(x.City) && x.City.ToUpper().Contains(criteria.City.ToUpper()));

                        if (criteria.State.IsNotNullOrEmpty())
                            query = query.Where(x => !string.IsNullOrEmpty(x.State) && x.State.ToUpper().Contains(criteria.State.ToUpper()));
                    }

                    // Switch to in-memory for complex string filters
                    var efMarket = query.AsEnumerable();

                    var pagination = mLister.Pagination ?? new Domain.Pagination();

                    pagination.TotalRecord = Math.Max(efMarket.Count(), 1);

                    if (pagination.Take <= 0)
                    {
                        pagination.Take = pagination.TotalRecord;
                        pagination.Skip = 0;
                    }

                    pagination.CurrentPage = pagination.Take > 0 ? (pagination.Skip / pagination.Take) + 1 : 1;

                    pagination.TotalPage = pagination.Take > 0 ? (int)Math.Ceiling((double)pagination.TotalRecord / pagination.Take) : 1;

                    efMarket = efMarket.Skip(pagination.Skip).Take(pagination.Take);

                    mLister.Pagination = pagination;

                    mLister.List = JsonSerializer.Deserialize<List<Domain.Market>>(JsonSerializer.Serialize(efMarket.ToList()));
                }
            }
            catch (Exception)
            {
                throw;
            }
            return mLister;
        }

        public Domain.Market Upsert(Domain.Market mMarket)
        {
            try
            {
                using (var context = new Persistence.CRMDBEntities())
                {
                    var efMarket = context.Markets.FirstOrDefault(x => x.Id == mMarket.Id);
                    if (efMarket == null)
                    {
                        var audit = new Persistence.AuditTrail
                        {
                            CreatedBy = mMarket.CreatedBy,
                            CreatedDate = DateTime.Now
                        };

                        context.AuditTrails.Add(audit);
                        context.SaveChanges();

                        efMarket = new Persistence.Market
                        {
                            AuditId = audit.Id
                        };
                        context.Markets.Add(efMarket);
                    }
                    else
                    {
                        var audit = context.AuditTrails.Find(efMarket.AuditId);
                        if (audit != null)
                        {
                            audit.LastModifiedBy = mMarket.LastModifiedBy;
                            audit.LastModifiedDate = DateTime.Now;
                        }
                    }

                    efMarket.Name = mMarket.Name;
                    efMarket.Line1 = mMarket.Line1;
                    efMarket.Line2 = mMarket.Line2;
                    efMarket.City = mMarket.City;
                    efMarket.State = mMarket.State;
                    efMarket.ZipCode = mMarket.ZipCode;
                    efMarket.Rank = mMarket.Rank;

                    context.SaveChanges();
                    mMarket.Id = efMarket.Id;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return mMarket;
        }

        public Domain.Market Get(int id)
        {
            var mMarket = new Domain.Market();
            try
            {
                using (var context = new Persistence.CRMDBEntities())
                {
                    var efMarket = (from m in context.Markets
                                    where m.Id == id
                                    select new
                                    {
                                        Id = m.Id,
                                        Name = m.Name,
                                        Line1 = m.Line1,
                                        Line2 = m.Line2,
                                        City = m.City,
                                        State = m.State,
                                        ZipCode = m.ZipCode,
                                        Rank = m.Rank,
                                    }).FirstOrDefault();

                    if (efMarket != null)
                    {
                        mMarket = JsonSerializer.Deserialize<Domain.Market>(JsonSerializer.Serialize(efMarket));
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return mMarket;
        }

        public void Delete(int id, int loginUserId)
        {
            try
            {
                using (var context = new Persistence.CRMDBEntities())
                {
                    var efMarket = context.Markets.FirstOrDefault(x => x.Id == id);
                    if (efMarket != null)
                    {
                        var audit = context.AuditTrails.Find(efMarket.AuditId);
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

        public bool CheckRankExist(Domain.Market mMarket)
        {
            bool isExist = false;
            try
            {
                using (var context = new Persistence.CRMDBEntities())
                {
                    //isExist = context.Markets.Where(x => x.Id != mMarket.Id && x.Rank.Trim().ToUpper() == mMarket.Rank.Trim().ToUpper()).Count() > 0;
                    isExist = context.Markets.Where(x => x.Id != mMarket.Id && x.Rank.ToString().Trim().ToUpper() == mMarket.Rank.ToString().Trim().ToUpper()).Count() > 0;

                }
            }
            catch (Exception)
            {
                throw;
            }
            return isExist;
        }

        public List<Domain.Market> GetActiveMarket()
        {
            var mMarket = new List<Domain.Market>();
            try
            {
                using (var context = new Persistence.CRMDBEntities())
                {
                    var efMarket = (from m in context.Markets
                                    join at in context.AuditTrails on m.AuditId equals at.Id
                                    where at.DeletedDate == null && at.DeletedBy == null
                                    select new
                                   {
                                       Id = m.Id,
                                       Name = m.Name,
                                   }).AsEnumerable();

                    mMarket = JsonSerializer.Deserialize<List<Domain.Market>>(JsonSerializer.Serialize(efMarket.ToList()));
                }
            }
            catch (Exception)
            {
                throw;
            }
            return mMarket;
        }

    }

}

