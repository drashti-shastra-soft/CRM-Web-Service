using CRMWebService.Infrastructure.IRepository;
using CRMWebService.Infrastructure.IService;
using System.Collections.Generic;

namespace CRMWebService.Service
{
    public class ReceiptAllocationService : IReceiptAllocationService
    {

        private readonly IReceiptAllocationRepository _repo;
        private readonly IInvoiceRepository _invoiceRepo;

        public ReceiptAllocationService(IReceiptAllocationRepository repo, IInvoiceRepository invoiceRepo)
        {
            _repo = repo;
            _invoiceRepo = invoiceRepo;
        }

        public Domain.ReceiptAllocationLister GetAll(Domain.ReceiptAllocationLister mLister)
        {
            return _repo.GetAll(mLister);
        }

        public Domain.ReceiptAllocation Upsert(Domain.ReceiptAllocation mReceiptAllocation)
        {
            return _repo.Upsert(mReceiptAllocation);
        }

        public List<Domain.ReceiptAllocation> Get(int id)
        {
            return _repo.Get(id);
        }

        public void Delete(int id, int loginUserId)
        {
            _repo.Delete(id, loginUserId);
        }
    }
}
