using CRMWebService.Infrastructure.IRepository;
using CRMWebService.Infrastructure.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMWebService.Service
{
    public class SalesReportService : ISalesReportService
    {
        private readonly ISalesReportRepository _repo;

        public SalesReportService(ISalesReportRepository repo)
        {
            _repo = repo;
        }

        public Domain.SalesReportLister GetAll(Domain.SalesReportLister mLister)
        {
            return _repo.GetAll(mLister);
        }

    }
}
