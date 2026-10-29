using System;
using System.Linq;

namespace CRMWebService.Domain
{
    public class Broker : AuditTrail
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ContactPerson { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string PANNumber { get; set; }
        public string AadharNumber { get; set; }
        public decimal Brokerage { get; set; }
        public bool IsActive { get; set; }
        public int AuditId { get; set; }
        public string Status { get; set; }

    }
}
