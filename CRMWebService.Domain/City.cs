using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMWebService.Domain
{
    public class City : AuditTrail
    {
        public int Id { get; set; }
        public int StateId { get; set; }
        public string State { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public int AuditId { get; set; }
        public string Status { get; set; }
    }
}
