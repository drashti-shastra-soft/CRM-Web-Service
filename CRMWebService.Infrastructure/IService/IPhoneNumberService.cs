using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMWebService.Infrastructure.IService
{
    public interface IPhoneNumberService
    {
        Domain.PhoneNumber Upsert(Domain.PhoneNumber mPhoneNumber);

        Domain.PhoneNumber Get(int id);
    }
}
