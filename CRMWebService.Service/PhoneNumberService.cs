using CRMWebService.Infrastructure.IRepository;
using CRMWebService.Infrastructure.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMWebService.Service
{
    public class PhoneNumberService : IPhoneNumberService
    {
        private readonly IPhoneNumberRepository _repo;

        public PhoneNumberService(IPhoneNumberRepository repo)
        {
            _repo = repo;
        }

        public Domain.PhoneNumber Upsert(Domain.PhoneNumber mPhoneNumber)
        {
            return _repo.Upsert(mPhoneNumber);
        }

        public Domain.PhoneNumber Get(int id)
        {
            return _repo.Get(id);
        }
    }
}
