using System;
using System.Collections.Generic;
using System.Linq;

namespace CRMWebService.Domain
{
    public class BrokerLister
    {
        public List<Broker> List { get; set; } = new List<Broker>();

        public Broker SearchCriteria { get; set; } = new Broker();

        public Pagination Pagination { get; set; } = new Pagination();
    }
}
