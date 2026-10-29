using CRMWebService.Infrastructure.IRepository;
using CRMWebService.Infrastructure.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMWebService.Service
{
    public class BankService : IBankService
    {
        private readonly IBankRepository _repo;

        public BankService(IBankRepository repo)
        {
            _repo = repo;
        }

        public Domain.BankLister GetAll(Domain.BankLister mLister)
        {
            return _repo.GetAll(mLister);
        }

        public Domain.Bank Upsert(Domain.Bank mBank)
        {
            return _repo.Upsert(mBank);
        }

        public Domain.Bank Get(int id)
        {
            return _repo.Get(id);
        }

        public void Delete(int id, int loginUserId)
        {
            _repo.Delete(id, loginUserId);
        }

        public List<Domain.Bank> GetActiveBank()
        {
            return _repo.GetActiveBank();
        }
    }
}
