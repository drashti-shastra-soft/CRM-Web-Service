using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMWebService.Infrastructure.IService
{
    public interface IReceiptService
    {
        Domain.ReceiptLister GetAll(Domain.ReceiptLister mLister);

        Domain.Receipt Upsert(Domain.Receipt mReceipt);

        Domain.Receipt Get(int id);

        void Delete(int id, int loginUserId);

        void PatchReceiptStatus(int id, string status, int loginUserId);


        //Domain.Receipt PatchReceiptStatus(int id, string status, int loginUserId);
    }
}
