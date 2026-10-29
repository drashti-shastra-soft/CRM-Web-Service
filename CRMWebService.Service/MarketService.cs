using CRMWebService.Infrastructure.IRepository;
using CRMWebService.Infrastructure.IService;
using System.Collections.Generic;

namespace CRMWebService.Service
{
    public class MarketService : IMarketService
    {
        private readonly IMarketRepository _repo;

        public MarketService(IMarketRepository repo)
        {
            _repo = repo;
        }

        public Domain.MarketLister GetAll(Domain.MarketLister mLister)
        {
            return _repo.GetAll(mLister);
        }

        public Domain.Market Upsert(Domain.Market mMarket)
        {
            return _repo.Upsert(mMarket);
        }

        public Domain.Market Get(int id)
        {
            return _repo.Get(id);
        }

        public void Delete(int id, int loginUserId)
        {
            _repo.Delete(id, loginUserId);
        }

        public bool CheckRankExist(Domain.Market mMarket)
        {
            return _repo.CheckRankExist(mMarket);
        }

        public List<Domain.Market> GetActiveMarket()
        {
            return _repo.GetActiveMarket();
        }
    }

}
