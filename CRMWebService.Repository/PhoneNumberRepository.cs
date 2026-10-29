using CRMWebService.Infrastructure.IRepository;
using System.Text.Json;
using System.Linq;
using System;

namespace CRMWebService.Repository
{
    public class PhoneNumberRepository : IPhoneNumberRepository
    {
        public Domain.PhoneNumber Upsert(Domain.PhoneNumber mPhoneNumber)
        {
            try
            {
                using (var context = new Persistence.CRMDBEntities())
                {
                    var efPhoneNumber = context.PhoneNumbers.FirstOrDefault(x => x.ClientId == mPhoneNumber.ClientId);

                    if (efPhoneNumber == null)
                    {
                        var audit = new Persistence.AuditTrail
                        {
                            CreatedBy = mPhoneNumber.CreatedBy,
                            CreatedDate = DateTime.Now
                        };

                        context.AuditTrails.Add(audit);
                        context.SaveChanges();

                        efPhoneNumber = new Persistence.PhoneNumber
                        {
                            AuditId = audit.Id
                        };

                        context.PhoneNumbers.Add(efPhoneNumber);
                    }
                    else
                    {
                        var audit = context.AuditTrails.Find(efPhoneNumber.AuditId);
                        if (audit != null)
                        {
                            audit.LastModifiedBy = mPhoneNumber.LastModifiedBy;
                            audit.LastModifiedDate = DateTime.Now;
                        }
                    }

                    efPhoneNumber.ClientId = mPhoneNumber.ClientId;
                    efPhoneNumber.PhoneNumber1 = mPhoneNumber.PhoneNumber1;
                    efPhoneNumber.PhoneNumber2 = mPhoneNumber.PhoneNumber2;
                    efPhoneNumber.PhoneNumber3 = mPhoneNumber.PhoneNumber3;
                    context.SaveChanges();
                    mPhoneNumber.Id = efPhoneNumber.Id;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return mPhoneNumber;
        }

        public Domain.PhoneNumber Get(int id)
        {
            var mPhoneNumber = new Domain.PhoneNumber();
            try
            {
                using (var context = new Persistence.CRMDBEntities())
                {
                    var efPhoneNumber = (from pn in context.PhoneNumbers
                                         where pn.ClientId == id
                                         select new
                                         {
                                             Id = pn.Id,
                                             ClientId = pn.ClientId,
                                             PhoneNumber1 = pn.PhoneNumber1,
                                             PhoneNumber2 = pn.PhoneNumber2,
                                             PhoneNumber3 = pn.PhoneNumber3
                                         }).FirstOrDefault();

                    if (efPhoneNumber != null)
                    {
                        mPhoneNumber = JsonSerializer.Deserialize<Domain.PhoneNumber>(JsonSerializer.Serialize(efPhoneNumber));
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return mPhoneNumber;
        }
    }
}
