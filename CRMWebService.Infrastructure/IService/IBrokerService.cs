using System;
using System.Collections.Generic;
using System.Linq;

namespace CRMWebService.Infrastructure.IService
{
    public interface IBrokerService
    {
        Domain.BrokerLister GetAll(Domain.BrokerLister mLister);

        Domain.Broker Upsert(Domain.Broker mBroker);

        Domain.Broker Get(int id);

        void Delete(int id, int loginUserId);

        List<Domain.Broker> GetActiveBroker();

    }
}
