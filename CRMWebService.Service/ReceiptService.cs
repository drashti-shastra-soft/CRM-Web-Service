using CRMWebService.Infrastructure.IRepository;
using CRMWebService.Infrastructure.IService;
using System;

namespace CRMWebService.Service
{
    public class ReceiptService : IReceiptService
    {
        private readonly IReceiptRepository _repo;
        private readonly IReceiptAllocationRepository _receiptAllocationRepo;
        private readonly IInvoiceRepository _invoiceRepo;
        private readonly IClientRepository _clientRepo;

        public ReceiptService(IReceiptRepository repo, IReceiptAllocationRepository receiptAllocationRepo, IInvoiceRepository invoiceRepo, IClientRepository clientRepo)
        {
            _repo = repo;
            _receiptAllocationRepo = receiptAllocationRepo;
            _invoiceRepo = invoiceRepo;
            _clientRepo = clientRepo;
        }

        public Domain.ReceiptLister GetAll(Domain.ReceiptLister mLister)
        {
            return _repo.GetAll(mLister);
        }

        public Domain.Receipt Upsert(Domain.Receipt mReceipt)
        {
            mReceipt = _repo.Upsert(mReceipt);
            if (mReceipt != null && mReceipt.Id > 0)
            {
                var mClient = _clientRepo.Get(mReceipt.ClientId);

                foreach (var item in mReceipt.ReceiptAllocations)
                {
                    item.ReceiptId = mReceipt.Id;
                    _receiptAllocationRepo.Upsert(item);

                    var mInvoice = _invoiceRepo.Get(item.InvoiceId);
                    if (mInvoice != null && mInvoice.Id > 0)
                    {
                        ApplyPayment(mInvoice, mClient, mReceipt.TotalPayment);
                        _invoiceRepo.UpdatePayment(mInvoice);
                    }
                }

            }
            return mReceipt;
        }

        public Domain.Receipt Get(int id)
        {
            return _repo.Get(id);
        }

        public void Delete(int id, int loginUserId)
        {
            _repo.Delete(id, loginUserId);
        }

        public void ApplyPayment(Domain.Invoice mInvoice, Domain.Client mClient, decimal paymentAmount)
        {
            mInvoice.TotalPayment += paymentAmount;
            decimal balance = mInvoice.NetAmount - mInvoice.TotalPayment;

            if (balance == 0)
            {
                mInvoice.Status = Convert.ToString(Infrastructure.Utils.Enums.InvoiceStatus.Paid);
            }
            else if (balance > 0)
            {
                mInvoice.Status = Convert.ToString(Infrastructure.Utils.Enums.InvoiceStatus.PartiallyPaid);
            }
            else
            {
                mInvoice.Status = Convert.ToString(Infrastructure.Utils.Enums.InvoiceStatus.Paid); // invoice is settled
                //mClient.Balance += Math.Abs(balance); // surplus stored as credit
            }
        }

        public void PatchReceiptStatus(int id, string status, int loginUserId)
        {
            _repo.PatchReceiptStatus(id, status, loginUserId);
        }



        //public Domain.Receipt PatchReceiptStatus(int id, string status, int loginUserId)
        //{
        //    return _repo.PatchReceiptStatus(id, status, loginUserId);
        //}



        //public void AdjustInvoiceWithBalance(Domain.Invoice invoice, Domain.Client account)
        //{
        //    if (account.Balance > 0)
        //    {
        //        decimal adjustment = Math.Min(account.Balance, invoice.NetAmount);
        //        invoice.TotalPayment += adjustment;
        //        account.Balance -= adjustment;
        //    }
        //}


    }
}
