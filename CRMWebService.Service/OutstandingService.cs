using CRMWebService.Infrastructure.IRepository;
using CRMWebService.Infrastructure.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMWebService.Service
{
    public class OutstandingService : IOutstandingService
    {
        private readonly IOutstandingRepository _repo;

        public OutstandingService(IOutstandingRepository repo)
        {
            _repo = repo;
        }

        public Domain.OutstandingLister GetAll(Domain.OutstandingLister mLister)
        {
            return _repo.GetAll(mLister);
        }
    }
}
