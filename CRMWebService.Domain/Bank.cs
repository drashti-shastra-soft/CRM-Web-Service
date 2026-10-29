using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMWebService.Domain
{
    public class Bank : AuditTrail
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string AccountType { get; set; }
        public string AccountNumber { get; set; }
        public string BranchName { get; set; }
        public string IFSCode { get; set; }
        public string UPI { get; set; }
        public string Status { get; set; }
        public bool IsActive { get; set; }
        public int AuditId { get; set; }
    }
}
