using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMWebService.Domain
{
    public class OutstandingLister
    {
        public List<Outstanding> List { get; set; } = new List<Outstanding>();

        public Outstanding SearchCriteria { get; set; } = new Outstanding();

        public Pagination Pagination { get; set; } = new Pagination();
    }
}
