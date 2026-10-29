using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMWebService.Domain
{
    public class ReceiptAllocation : AuditTrail
    {
        public int Id { get; set; }
        public int ReceiptId { get; set; }
        public int InvoiceId { get; set; }
        public string InvoiceNumber { get; set; }
        public decimal Amount { get; set; }
        public decimal NetAmount { get; set; }
        public decimal TotalPayment { get; set; }
        public decimal FinalAmount { get; set; }
        public string Status { get; set; }
        
        public decimal AmountDue
        {
            get { return NetAmount - TotalPayment; }
        }
        
    }
}
