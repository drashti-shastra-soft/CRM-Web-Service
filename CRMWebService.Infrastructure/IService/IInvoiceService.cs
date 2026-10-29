using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMWebService.Infrastructure.IService
{
    public interface IInvoiceService
    {
        Domain.InvoiceLister GetAll(Domain.InvoiceLister mLister);

        Domain.Invoice Upsert(Domain.Invoice mInvoice);

        Domain.Invoice Get(int id);

        List<Domain.Invoice> GetBy(int clientId);

        void Delete(int id, int loginUserId);

        //void UpdatePayment(Domain.Invoice mInvoice);
    }
}
