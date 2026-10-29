using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMWebService.Domain
{
    public class SalesReceiptLister
    {
        public List<SalesReceipt> List { get; set; } = new List<SalesReceipt>();

        public SalesReceipt SearchCriteria { get; set; } = new SalesReceipt();

        public Pagination Pagination { get; set; } = new Pagination();
    }
}
