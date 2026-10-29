using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMWebService.Domain
{
    public class AddressLister
    {
        public List<Address> List { get; set; } = new List<Address>();

        public Address SearchCriteria { get; set; } = new Address();

        public Pagination Pagination { get; set; } = new Pagination();
    }
}
