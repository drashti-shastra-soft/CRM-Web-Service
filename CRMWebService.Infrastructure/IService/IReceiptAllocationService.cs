using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMWebService.Infrastructure.IService
{
    public interface IReceiptAllocationService
    {
        Domain.ReceiptAllocationLister GetAll(Domain.ReceiptAllocationLister mLister);

        Domain.ReceiptAllocation Upsert(Domain.ReceiptAllocation mReceiptAllocation);

        List<Domain.ReceiptAllocation> Get(int id);

        void Delete(int id, int loginUserId);
    }
}
