using System;
using System.Linq;

namespace CRMWebService.Domain
{
    public class Client : AuditTrail
    {
        public int Id { get; set; }
        public int DyeingSupervisorId { get; set; }
        public string DyeingSupervisor { get; set; }
        public int CollectionAgentId { get; set; }
        public string CollectionAgent { get; set; }
        public int? BrokerId { get; set; }
        public int ClientTypeId { get; set; }
        public string ClientType { get; set; }
        public string Brand { get; set; }
        public string Name { get; set; }
        public string ContactPerson { get; set; }
        public string EmailAddress { get; set; }
        public string MobileNumber { get; set; }
        public string GSTNumber { get; set; }
        public string PANNumber { get; set; }
        public string AadharNumber { get; set; }
        public string TANNumber { get; set; }
        public decimal CreditLimit { get; set; }
        public string PaymentTerms { get; set; }
        public bool IsActive { get; set; } = true;
        public string Status { get; set; }
        public int? AddressId { get; set; }
        public int AuditId { get; set; }
    }
}
