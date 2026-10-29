using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMWebService.Infrastructure.IRepository
{
    public interface IInvoiceLineItemRepository
    {
        void Create(Domain.InvoiceLineItem mInvoiceLineItem);

        List<Domain.InvoiceLineItem> Get(int id);

        void Delete(int id, int loginUserId);

        void DeleteByInvoiceId(int invoiceId, int loginUserId);
    }
}
