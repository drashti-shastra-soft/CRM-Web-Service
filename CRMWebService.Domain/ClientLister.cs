using System;
using System.Collections.Generic;
using System.Linq;

namespace CRMWebService.Domain
{
    public class ClientLister
    {
        public List<Client> List { get; set; } = new List<Client>();

        public Client SearchCriteria { get; set; } = new Client();

        public Pagination Pagination { get; set; } = new Pagination();
    }
}
