using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMWebService.Domain
{
    public class Receipt : AuditTrail
    {
        public int Id { get; set; }
        public int VoucherNumber { get; set; }
        public int CollectionAgentId { get; set; }
        public string CollectionAgent { get; set; }
        public int ClientId { get; set; }
        public DateTime VoucherDate { get; set; }
        public string Method { get; set; }
        public string Client { get; set; }
        public decimal TotalPayment { get; set; }
        public decimal CashDiscount { get; set; }
        public decimal TDS { get; set; }
        public decimal RateDifference { get; set; }
        public decimal ClaimAmount { get; set; }
        public decimal FinalAmount { get; set; }
        public string ReferenceNumber { get; set; }
        public int BankId { get; set; }
        public string Status { get; set; }
        public DateTime? ChequeDate { get; set; }
        public int AuditId { get; set; }
        public List<Domain.ReceiptAllocation> ReceiptAllocations { get; set; } = new List<Domain.ReceiptAllocation>();

    }
}
