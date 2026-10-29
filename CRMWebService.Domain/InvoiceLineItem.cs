using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMWebService.Domain
{
    public class InvoiceLineItem : AuditTrail
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string Style { get; set; }
        public string LotNumber { get; set; }
        public decimal GreyMeter { get; set; }
        public int Piece { get; set; }
        public int? PartPiece { get; set; }
        public decimal GreyValue { get; set; }
        public decimal FinishMeter { get; set; }
        public decimal? Shortage { get; set; }
        public decimal Rate { get; set; }
        public decimal Amount { get; set; }
        public decimal? AddToTotal { get; set; }
        public decimal? SubtractFromTotal { get; set; }
    }
}
