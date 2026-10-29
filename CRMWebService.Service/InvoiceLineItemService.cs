using CRMWebService.Infrastructure.IRepository;
using CRMWebService.Infrastructure.IService;
using System.Collections.Generic;

namespace CRMWebService.Service
{
    public class InvoiceLineItemService : IInvoiceLineItemService
    {
        private readonly IInvoiceLineItemRepository _repo;

        public InvoiceLineItemService(IInvoiceLineItemRepository repo)
        {
            _repo = repo;
        }

        public void Create(Domain.InvoiceLineItem mInvoiceLineItem)
        {
            _repo.Create(mInvoiceLineItem);
        }

        public List<Domain.InvoiceLineItem> Get(int id)
        {
            return _repo.Get(id);
        }

        public void Delete(int id, int loginUserId)
        {
            _repo.Delete(id, loginUserId);
        }
    }
}
