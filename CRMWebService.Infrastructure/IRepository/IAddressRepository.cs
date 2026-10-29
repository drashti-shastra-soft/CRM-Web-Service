using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMWebService.Infrastructure.IRepository
{
    public interface IAddressRepository
    {
        Domain.AddressLister GetAll(Domain.AddressLister mLister);

        Domain.Address Upsert(Domain.Address mAddress);

        Domain.Address Get(int id);

        void Delete(int id, int loginUserId);
    }
}
