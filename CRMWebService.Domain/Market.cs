using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMWebService.Domain
{
    public class Market : AuditTrail
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public int Rank { get; set; }
        public int AuditId { get; set; }
        public string Status { get; set; }
    }

}
