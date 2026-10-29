using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMWebService.Domain
{
    public class ReceiptLister
    {
        public List<Receipt> List { get; set; } = new List<Receipt>();

        public Receipt SearchCriteria { get; set; } = new Receipt();

        public Pagination Pagination { get; set; } = new Pagination();
    }
}
