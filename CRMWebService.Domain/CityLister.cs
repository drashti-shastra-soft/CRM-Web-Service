using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMWebService.Domain
{
    public class CityLister
    {
        public List<City> List { get; set; } = new List<City>();

        public City SearchCriteria { get; set; } = new City();

        public Pagination Pagination { get; set; } = new Pagination();
    }
}
