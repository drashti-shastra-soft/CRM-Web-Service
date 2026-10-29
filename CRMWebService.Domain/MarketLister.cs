using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMWebService.Domain
{
    public class MarketLister
    {
        public List<Market> List { get; set; } = new List<Market>();

        public Market SearchCriteria { get; set; } = new Market();

        public Pagination Pagination { get; set; } = new Pagination();
    }

}
