using CRMWebService.Infrastructure.IRepository;
using CRMWebService.Infrastructure.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMWebService.Service
{
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository _repo;

        public AddressService(IAddressRepository repo)
        {
            _repo = repo;
        }

        public Domain.AddressLister GetAll(Domain.AddressLister mLister)
        {
            return _repo.GetAll(mLister);
        }

        public Domain.Address Upsert(Domain.Address mAddress)
        {
            return _repo.Upsert(mAddress);
        }

        public Domain.Address Get(int id)
        {
            return _repo.Get(id);
        }

        public void Delete(int id, int loginUserId)
        {
            _repo.Delete(id, loginUserId);
        }
    }
}
