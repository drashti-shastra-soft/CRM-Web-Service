using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMWebService.Infrastructure.Utils
{
    public static class Enums
    {
        public enum UserType
        {
            System_Administrator = 1,
            Business_Admin = 2,
            Staff_Member = 3,
            Dyeing_Supervisor = 4,
            Collection_Agent = 5,
            Client = 6
        }

        public enum InvoiceStatus
        {
            Pending,
            PartiallyPaid,
            Paid,
            Overpaid
        }

    }

}
