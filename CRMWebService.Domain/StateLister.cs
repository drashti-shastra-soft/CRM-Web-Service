using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMWebService.Domain
{
    public class StateLister
    {
        public List<State> List { get; set; } = new List<State>();

        public State SearchCriteria { get; set; } = new State();

        public Pagination Pagination { get; set; } = new Pagination();
    }
}
