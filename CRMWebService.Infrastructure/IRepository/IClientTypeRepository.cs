using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMWebService.Infrastructure.IRepository
{
    public interface IClientTypeRepository
    {
        List<Domain.ClientType> GetActiveClientTypes();
    }
}
