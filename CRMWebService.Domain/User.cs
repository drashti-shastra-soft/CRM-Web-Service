using System;
using System.Linq;

namespace CRMWebService.Domain
{
    public class User : AuditTrail
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public int ClientTypeId { get; set; }
        public string Brand { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string MobileNumber { get; set; }
        public string Password { get; set; }
        public int UserTypeId { get; set; }
        public string UserType { get; set; }
        public string PlainTextPassword { get; set; }
        public bool IsActive { get; set; }
        public string Status { get; set; }
        public string Token { get; set; }
        public int AuditId { get; set; }
    }
}
