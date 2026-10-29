using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMWebService.Infrastructure.IRepository
{
    public interface IMarketRepository
    {
        Domain.MarketLister GetAll(Domain.MarketLister mLister);

        Domain.Market Upsert(Domain.Market mMarket);

        Domain.Market Get(int id);

        void Delete(int id, int loginUserId);

        bool CheckRankExist(Domain.Market mMarket);

        List<Domain.Market> GetActiveMarket();

    }

}
