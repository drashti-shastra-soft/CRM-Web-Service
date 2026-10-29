using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMWebService.Domain
{
    public class ReceiptAllocationLister
    {
        public List<ReceiptAllocation> List { get; set; } = new List<ReceiptAllocation>();

        public ReceiptAllocation SearchCriteria { get; set; } = new ReceiptAllocation();

        public Pagination Pagination { get; set; } = new Pagination();
    }
}
