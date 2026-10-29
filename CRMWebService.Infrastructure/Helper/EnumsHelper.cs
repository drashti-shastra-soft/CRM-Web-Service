using CRMWebService.Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMWebService.Infrastructure.Helper
{
    public static class EnumsHelper
    {
        public static int GetEnumIndex(Enums.UserType role)
        {
            switch (role)
            {
                case Enums.UserType.System_Administrator:
                    return (int)Enums.UserType.System_Administrator;
                case Enums.UserType.Business_Admin:
                    return (int)Enums.UserType.Business_Admin;
                case Enums.UserType.Staff_Member:
                    return (int)Enums.UserType.Staff_Member;
                case Enums.UserType.Dyeing_Supervisor:
                    return (int)Enums.UserType.Dyeing_Supervisor;
                case Enums.UserType.Collection_Agent:
                    return (int)Enums.UserType.Collection_Agent;
                case Enums.UserType.Client:
                    return (int)Enums.UserType.Client;
                default:
                    throw new ArgumentOutOfRangeException(nameof(role), role, null);
            }
        }
    }
}
