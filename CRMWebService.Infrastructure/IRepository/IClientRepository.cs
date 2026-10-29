using System;
using System.Collections.Generic;
using System.Linq;

namespace CRMWebService.Infrastructure.IRepository
{
    public interface IClientRepository
    {
        Domain.ClientLister GetAll(Domain.ClientLister mLister);

        Domain.Client Upsert(Domain.Client mClient);

        Domain.Client Get(int id);

        void Delete(int id, int loginUserId);

        List<Domain.Client> GetActiveClientTypes();

        List<Domain.Client> GetActiveClient();
    }
}
