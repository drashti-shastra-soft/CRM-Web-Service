using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMWebService.Domain
{
    public class BankLister
    {
        public List<Bank> List { get; set; } = new List<Bank>();

        public Bank SearchCriteria { get; set; } = new Bank();

        public Pagination Pagination { get; set; } = new Pagination();
    }
}
