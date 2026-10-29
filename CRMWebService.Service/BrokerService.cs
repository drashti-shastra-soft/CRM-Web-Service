using CRMWebService.Infrastructure.IRepository;
using CRMWebService.Infrastructure.IService;
using System.Collections.Generic;


namespace CRMWebService.Service
{
    public class BrokerService : IBrokerService
    {
        private readonly IBrokerRepository _repo;

        public BrokerService(IBrokerRepository repo)
        {
            _repo = repo;
        }

        public Domain.BrokerLister GetAll(Domain.BrokerLister mLister)
        {
            return _repo.GetAll(mLister);
        }

        public Domain.Broker Upsert(Domain.Broker mBroker)
        {
            return _repo.Upsert(mBroker);
        }

        public Domain.Broker Get(int id)
        {
            return _repo.Get(id);
        }

        public void Delete(int id, int loginUserId)
        {
            _repo.Delete(id, loginUserId);
        }

        public List<Domain.Broker> GetActiveBroker()
        {
            return _repo.GetActiveBroker();
        }

    }
}
