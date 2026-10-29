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
    public class BankRepository : IBankRepository
    {
        public Domain.BankLister GetAll(Domain.BankLister mLister)
        {
            try
            {
                using (var context = new Persistence.CRMDBEntities())
                {
                    var query = from ba in context.Banks
                                join at in context.AuditTrails on ba.AuditId equals at.Id
                                where at.DeletedBy == null && at.DeletedDate == null
                                orderby ba.Id descending
                                select new
                                {
                                    Id = ba.Id,
                                    Name = ba.Name,
                                    AccountType = ba.AccountType,
                                    AccountNumber = ba.AccountNumber,
                                    BranchName = ba.BranchName,
                                    IFSCode = ba.IFSCode,
                                    UPI = ba.UPI,
                                    IsActive = ba.IsActive,
                                };


                    if (mLister.SearchCriteria != null)
                    {
                        var criteria = mLister.SearchCriteria;

                        if (criteria.Name.IsNotNullOrEmpty())
                            query = query.Where(x => !string.IsNullOrEmpty(x.Name) && x.Name.ToUpper().Contains(criteria.Name.ToUpper()));

                        if (criteria.AccountType.IsNotNullOrEmpty())
                            query = query.Where(x => !string.IsNullOrEmpty(x.AccountType) && x.AccountType.ToUpper().Contains(criteria.AccountType.ToUpper()));

                        if (criteria.AccountNumber.IsNotNullOrEmpty())
                            query = query.Where(x => !string.IsNullOrEmpty(x.AccountNumber) && x.AccountNumber.ToUpper().Contains(criteria.AccountNumber.ToUpper()));

                        if (criteria.BranchName.IsNotNullOrEmpty())
                            query = query.Where(x => !string.IsNullOrEmpty(x.BranchName) && x.BranchName.ToUpper().Contains(criteria.BranchName.ToUpper()));

                        if (criteria.IFSCode.IsNotNullOrEmpty())
                            query = query.Where(x => !string.IsNullOrEmpty(x.IFSCode) && x.IFSCode.ToUpper().Contains(criteria.IFSCode.ToUpper()));

                        if (criteria.Status.IsNotNullOrEmpty())
                        {
                            if (criteria.Status == "Active")
                                query = query.Where(x => x.IsActive);
                            else if (criteria.Status == "Disabled")
                                query = query.Where(x => !x.IsActive);
                        }
                    }

                    // Switch to in-memory for complex string filters
                    var efBank = query.AsEnumerable();

                    var pagination = mLister.Pagination ?? new Domain.Pagination();

                    pagination.TotalRecord = Math.Max(efBank.Count(), 1);

                    if (pagination.Take <= 0)
                    {
                        pagination.Take = pagination.TotalRecord;
                        pagination.Skip = 0;
                    }

                    pagination.CurrentPage = pagination.Take > 0 ? (pagination.Skip / pagination.Take) + 1 : 1;

                    pagination.TotalPage = pagination.Take > 0 ? (int)Math.Ceiling((double)pagination.TotalRecord / pagination.Take) : 1;

                    efBank = efBank.Skip(pagination.Skip).Take(pagination.Take);

                    mLister.Pagination = pagination;
                    mLister.List = JsonSerializer.Deserialize<List<Domain.Bank>>(JsonSerializer.Serialize(efBank.ToList()));
                }
            }
            catch (Exception)
            {
                throw;
            }
            return mLister;
        }

        public Domain.Bank Upsert(Domain.Bank mBank)
        {
            try
            {
                using (var context = new Persistence.CRMDBEntities())
                {
                    var efBank = context.Banks.FirstOrDefault(x => x.Id == mBank.Id);
                    if (efBank == null)
                    {
                        var audit = new Persistence.AuditTrail
                        {
                            CreatedBy = mBank.CreatedBy,
                            CreatedDate = DateTime.Now
                        };

                        context.AuditTrails.Add(audit);
                        context.SaveChanges();

                        efBank = new Persistence.Bank
                        {
                            AuditId = audit.Id
                        };
                        context.Banks.Add(efBank);
                    }
                    else
                    {
                        var audit = context.AuditTrails.Find(efBank.AuditId);
                        if (audit != null)
                        {
                            audit.LastModifiedBy = mBank.LastModifiedBy;
                            audit.LastModifiedDate = DateTime.Now;
                        }
                    }

                    efBank.Name = mBank.Name;
                    efBank.AccountType = mBank.AccountType;
                    efBank.AccountNumber = mBank.AccountNumber;
                    efBank.BranchName = mBank.BranchName;
                    efBank.IFSCode = mBank.IFSCode;
                    efBank.UPI = mBank.UPI;
                    efBank.IsActive = mBank.IsActive;

                    context.SaveChanges();

                    mBank.Id = efBank.Id;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return mBank;
        }

        public Domain.Bank Get(int id)
        {
            var mBank = new Domain.Bank();
            try
            {
                using (var context = new Persistence.CRMDBEntities())
                {
                    var efBank = (from ba in context.Banks
                                  where ba.Id == id
                                  select new
                                  {
                                      Id = ba.Id,
                                      Name = ba.Name,
                                      AccountType = ba.AccountType,
                                      AccountNumber = ba.AccountNumber,
                                      BranchName = ba.BranchName,
                                      IFSCode = ba.IFSCode,
                                      UPI = ba.UPI,
                                      IsActive = ba.IsActive,
                                  }).FirstOrDefault();

                    if (efBank != null)
                    {
                        mBank = JsonSerializer.Deserialize<Domain.Bank>(JsonSerializer.Serialize(efBank));
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return mBank;
        }

        public void Delete(int id, int loginUserId)
        {
            try
            {
                using (var context = new Persistence.CRMDBEntities())
                {
                    var efBank = context.Banks.FirstOrDefault(x => x.Id == id);
                    if (efBank != null)
                    {
                        var audit = context.AuditTrails.Find(efBank.AuditId);
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

        public List<Domain.Bank> GetActiveBank()
        {
            var mBank = new List<Domain.Bank>();
            try
            {
                using (var context = new Persistence.CRMDBEntities())
                {
                    var efBank = (from ba in context.Banks
                                  join at in context.AuditTrails on ba.AuditId equals at.Id
                                  where at.DeletedDate == null && at.DeletedBy == null && ba.IsActive
                                  select new
                                  {
                                      Id = ba.Id,
                                      Name = ba.Name,
                                  }).AsEnumerable();

                    mBank = JsonSerializer.Deserialize<List<Domain.Bank>>(JsonSerializer.Serialize(efBank.ToList()));
                }
            }
            catch (Exception)
            {
                throw;
            }
            return mBank;
        }
    }
}
