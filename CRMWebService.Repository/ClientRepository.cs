using CRMWebService.Infrastructure.IRepository;
using CRMWebService.Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace CRMWebService.Repository
{
    public class ClientRepository : IClientRepository
    {
        public Domain.ClientLister GetAll(Domain.ClientLister mLister)
        {
            try
            {
                using (var context = new Persistence.CRMDBEntities())
                {
                    var query = from c in context.Clients
                                join ds in context.Users on c.DyeingSupervisorId equals ds.Id
                                join ca in context.Users on c.CollectionAgentId equals ca.Id
                                join ct in context.ClientTypes on c.ClientTypeId equals ct.Id
                                join at in context.AuditTrails on c.AuditId equals at.Id
                                where at.DeletedBy == null && at.DeletedDate == null
                                select new
                                {
                                    Id = c.Id,
                                    DyeingSupervisorId = c.DyeingSupervisorId,
                                    DyeingSupervisor = ds.FirstName + " " + ds.LastName,
                                    CollectionAgentId = c.CollectionAgentId,
                                    CollectionAgent = ca.FirstName + " " + ca.LastName,
                                    ClientTypeId = c.ClientTypeId,
                                    ClientType = ct.Name,
                                    Brand = c.Brand,
                                    Name = c.Name,
                                    ContactPerson = c.ContactPerson,
                                    EmailAddress = c.EmailAddress,
                                    MobileNumber = c.MobileNumber,
                                    GSTNumber = c.GSTNumber,
                                    PANNumber = c.PANNumber,
                                    AadharNumber = c.AadharNumber,
                                    TANNumber = c.TANNumber,
                                    CreditLimit = c.CreditLimit,
                                    PaymentTerms = c.PaymentTerms,
                                    AddressId = c.AddressId,
                                    AuditId = c.AuditId,
                                    BrokerId = c.BrokerId,
                                    IsActive = c.IsActive,
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

                        if (criteria.DyeingSupervisor.IsNotNullOrEmpty())
                            query = query.Where(x => !string.IsNullOrEmpty(x.DyeingSupervisor) && x.DyeingSupervisor.ToUpper().Contains(criteria.DyeingSupervisor.ToUpper()));

                        if (criteria.CollectionAgent.IsNotNullOrEmpty())
                            query = query.Where(x => !string.IsNullOrEmpty(x.CollectionAgent) && x.CollectionAgent.ToUpper().Contains(criteria.CollectionAgent.ToUpper()));

                        if (criteria.ClientType.IsNotNullOrEmpty())
                            query = query.Where(x => !string.IsNullOrEmpty(x.ClientType) && x.ClientType.ToUpper().Contains(criteria.ClientType.ToUpper()));

                        if (criteria.Brand.IsNotNullOrEmpty())
                            query = query.Where(x => !string.IsNullOrEmpty(x.Brand) && x.Brand.ToUpper().Contains(criteria.Brand.ToUpper()));

                        if (criteria.Name.IsNotNullOrEmpty())
                            query = query.Where(x => !string.IsNullOrEmpty(x.Name) && x.Name.ToUpper().Contains(criteria.Name.ToUpper()));

                        if (criteria.MobileNumber.IsNotNullOrEmpty())
                            query = query.Where(x => !string.IsNullOrEmpty(x.MobileNumber) && x.MobileNumber.ToUpper().Contains(criteria.MobileNumber.ToUpper()));

                        if (criteria.GSTNumber.IsNotNullOrEmpty())
                            query = query.Where(x => !string.IsNullOrEmpty(x.GSTNumber) && x.GSTNumber.ToUpper().Contains(criteria.GSTNumber.ToUpper()));

                    }
                    // Switch to in-memory for complex string filters
                    var efClient = query.AsEnumerable();

                    var pagination = mLister.Pagination ?? new Domain.Pagination();

                    pagination.TotalRecord = Math.Max(efClient.Count(), 1);

                    if (pagination.Take <= 0)
                    {
                        pagination.Take = pagination.TotalRecord;
                        pagination.Skip = 0;
                    }

                    pagination.CurrentPage = pagination.Take > 0 ? (pagination.Skip / pagination.Take) + 1 : 1;

                    pagination.TotalPage = pagination.Take > 0 ? (int)Math.Ceiling((double)pagination.TotalRecord / pagination.Take) : 1;

                    efClient = efClient.Skip(pagination.Skip).Take(pagination.Take);

                    mLister.Pagination = pagination;
                    mLister.List = JsonSerializer.Deserialize<List<Domain.Client>>(JsonSerializer.Serialize(efClient.ToList()));

                }
            }
            catch
            {
                throw;
            }

            return mLister;

        }

        public Domain.Client Upsert(Domain.Client mClient)
        {
            try
            {
                using ( var context = new Persistence.CRMDBEntities())
                {
                    var efClient = context.Clients.FirstOrDefault(x => x.Id == mClient.Id);
                    if (efClient == null)
                    {
                        var audit = new Persistence.AuditTrail
                        {
                            CreatedBy = mClient.CreatedBy,
                            CreatedDate = DateTime.Now
                        };

                        context.AuditTrails.Add(audit);
                        context.SaveChanges();

                        efClient = new Persistence.Client
                        {
                            AuditId = audit.Id
                        };
                        context.Clients.Add(efClient);
                    }
                    else
                    {
                        var audit = context.AuditTrails.Find(efClient.AuditId);
                        if (audit != null)
                        {
                            audit.LastModifiedBy = mClient.LastModifiedBy;
                            audit.LastModifiedDate = DateTime.Now;
                        }
                    }
                    efClient.DyeingSupervisorId = mClient.DyeingSupervisorId;
                    efClient.CollectionAgentId = mClient.CollectionAgentId;
                    efClient.BrokerId = mClient.BrokerId;
                    efClient.ClientTypeId = mClient.ClientTypeId;
                    efClient.Brand = mClient.Brand;
                    efClient.Name = mClient.Name;
                    efClient.ContactPerson = mClient.ContactPerson;
                    efClient.EmailAddress = mClient.EmailAddress;
                    efClient.MobileNumber = mClient.MobileNumber;
                    efClient.GSTNumber = mClient.GSTNumber;
                    efClient.PANNumber = mClient.PANNumber;
                    efClient.AadharNumber = mClient.AadharNumber;
                    efClient.TANNumber = mClient.TANNumber;
                    efClient.CreditLimit = mClient.CreditLimit;
                    efClient.PaymentTerms = mClient.PaymentTerms;
                    efClient.IsActive = mClient.IsActive;
                    efClient.AddressId = mClient.AddressId;

                    context.SaveChanges();

                    mClient.Id = efClient.Id;
                }
            }
            catch (Exception)
            {
                throw;
            }

            return mClient;
        }

        public Domain.Client Get(int id)
        {
            var mClient = new Domain.Client();
            try
            {
                using (var context = new Persistence.CRMDBEntities())
                {
                    var efClient = (from c in context.Clients
                                    where c.Id == id
                                    select new
                                    {
                                        Id = c.Id,
                                        DyeingSupervisorId = c.DyeingSupervisorId,
                                        CollectionAgentId = c.CollectionAgentId,
                                        ClientTypeId = c.ClientTypeId,
                                        Brand = c.Brand,
                                        Name = c.Name,
                                        ContactPerson = c.ContactPerson,
                                        EmailAddress = c.EmailAddress,
                                        MobileNumber = c.MobileNumber,
                                        GSTNumber = c.GSTNumber,
                                        PANNumber = c.PANNumber,
                                        AadharNumber = c.AadharNumber,
                                        TANNumber = c.TANNumber,
                                        CreditLimit = c.CreditLimit,
                                        PaymentTerms = c.PaymentTerms,
                                        AddressId = c.AddressId,
                                        AuditId = c.AuditId,
                                        BrokerId = c.BrokerId,
                                        IsActive = c.IsActive,
                                    }).FirstOrDefault();

                    if (efClient != null)
                    {
                        mClient = JsonSerializer.Deserialize<Domain.Client>(JsonSerializer.Serialize(efClient));
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return mClient;
        }

        public void Delete(int id, int loginUserId)
        {
            try
            {
                using (var context = new Persistence.CRMDBEntities())
                {
                    var efClient = context.Clients.FirstOrDefault(x => x.Id == id);
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

        public List<Domain.Client> GetActiveClientTypes()
        {
            var mClient = new List<Domain.Client>();
            try
            {
                using (var context = new Persistence.CRMDBEntities())
                {
                    var efClient = (from c in context.Clients
                                    join ct in context.ClientTypes on c.ClientTypeId equals ct.Id
                                    where c.IsActive
                                    select new
                                    {
                                        Id = c.Id,
                                        Name = c.Name,
                                    }).AsEnumerable();

                    mClient = JsonSerializer.Deserialize<List<Domain.Client>>(JsonSerializer.Serialize(efClient.ToList()));
                }
            }
            catch (Exception)
            {
                throw;
            }
            return mClient;
        }

        public List<Domain.Client> GetActiveClient()
        {
            var mClient = new List<Domain.Client>();
            try
            {
                using (var context = new Persistence.CRMDBEntities())
                {
                    var efClient = (from c in context.Clients
                                    join at in context.AuditTrails on c.AuditId equals at.Id
                                    where at.DeletedDate == null && at.DeletedBy == null && c.IsActive
                                    select new
                                    {
                                        Id = c.Id,
                                        Name = c.Name,
                                    }).AsEnumerable();

                    mClient = JsonSerializer.Deserialize<List<Domain.Client>>(JsonSerializer.Serialize(efClient.ToList()));
                }
            }
            catch (Exception)
            {
                throw;
            }
            return mClient;
        }
    }
}
