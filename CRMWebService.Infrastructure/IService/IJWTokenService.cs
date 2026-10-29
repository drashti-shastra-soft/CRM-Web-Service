using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMWebService.Infrastructure.IService
{
    public interface IJWTokenService
    {
        string GenerateJwtToken(Domain.User mUser);
    }
}
