using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMWebService.Infrastructure.IService
{
    public interface IMarketService
    {
        Domain.MarketLister GetAll(Domain.MarketLister mLister);

        Domain.Market Upsert(Domain.Market mMarket);

        Domain.Market Get(int id);

        void Delete(int id, int loginUserId);

        List<Domain.Market> GetActiveMarket();

        bool CheckRankExist(Domain.Market mMarket);
    }

}
