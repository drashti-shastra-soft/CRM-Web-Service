using CRMWebService.Infrastructure.IRepository;
using CRMWebService.Infrastructure.IService;
using System.Collections.Generic;


namespace CRMWebService.Service
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IInvoiceRepository _repo;
        private readonly IInvoiceLineItemRepository _invoiceLineItemRepo;

        public InvoiceService(IInvoiceRepository repo, IInvoiceLineItemRepository invoiceLineItemRepo)
        {
            _repo = repo;
            _invoiceLineItemRepo = invoiceLineItemRepo;
        }

        public Domain.InvoiceLister GetAll(Domain.InvoiceLister mLister)
        {
            return _repo.GetAll(mLister);
        }

        public Domain.Invoice Upsert(Domain.Invoice mInvoice)
        {
            mInvoice = _repo.Upsert(mInvoice);
            if (mInvoice != null && mInvoice.Id > 0)
            {
                foreach (var item in mInvoice.LineItems)
                {
                    item.InvoiceId = mInvoice.Id;
                    _invoiceLineItemRepo.Create(item);
                }
            }
            return mInvoice;
        }

        public Domain.Invoice Get(int id)
        {
            return _repo.Get(id);
        }

        public List<Domain.Invoice> GetBy(int clientId)
        {
            return _repo.GetBy(clientId);
        }

        public void Delete(int id, int loginUserId)
        {
            _repo.Delete(id, loginUserId);
            _invoiceLineItemRepo.DeleteByInvoiceId(id, loginUserId);
        }

        //public void UpdatePayment(Domain.Invoice mInvoice)
        //{
        //    _repo.UpdatePayment(mInvoice);
        //}
    }
}
