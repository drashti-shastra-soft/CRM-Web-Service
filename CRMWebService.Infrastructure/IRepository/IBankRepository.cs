using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMWebService.Infrastructure.IRepository
{
    public interface IBankRepository
    {
        Domain.BankLister GetAll(Domain.BankLister mLister);

        Domain.Bank Upsert(Domain.Bank mBank);

        Domain.Bank Get(int id);

        void Delete(int id, int loginUserId);

        List<Domain.Bank> GetActiveBank();
    }
}
