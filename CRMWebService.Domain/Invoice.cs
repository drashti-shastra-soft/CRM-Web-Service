using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMWebService.Domain
{
    public class Invoice : AuditTrail
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string BookType { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public string InvoiceNumber { get; set; }
        public string ChallanNumber { get; set; }
        public string LRNumber { get; set; }
        public string VehicleNumber { get; set; }
        public int DyeingSupervisorId { get; set; }
        public string DyeingSupervisorName { get; set; }
        public int CollectionAgentId { get; set; }
        public string CollectionAgentName { get; set; }
        public int? BrokerId { get; set; }
        public string BrokerName { get; set; }
        public decimal TaxableValue { get; set; }
        public decimal? SGST { get; set; }
        public decimal? CGST { get; set; }
        public decimal? IGST { get; set; }
        public decimal NetAmount { get; set; }
        public string Status { get; set; }
        public decimal TotalPayment { get; set; }
        public decimal AmountDue
        {
            get { return NetAmount - TotalPayment; }
        }
        public List<Domain.InvoiceLineItem> LineItems { get; set; } = new List<Domain.InvoiceLineItem>();
    }
}
