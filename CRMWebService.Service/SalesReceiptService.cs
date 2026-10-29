using CRMWebService.Infrastructure.IRepository;
using CRMWebService.Infrastructure.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMWebService.Service
{
    public class SalesReceiptService : ISalesReceiptService
    {
        private readonly ISalesReceiptRepository _repo;

        public SalesReceiptService(ISalesReceiptRepository repo)
        {
            _repo = repo;
        }

        public Domain.SalesReceiptLister GetAll(Domain.SalesReceiptLister mLister)
        {
            return _repo.GetAll(mLister);
        }
    }
}
