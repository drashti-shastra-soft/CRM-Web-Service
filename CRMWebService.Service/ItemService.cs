using CRMWebService.Infrastructure.IRepository;
using CRMWebService.Infrastructure.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMWebService.Service
{
    public class ItemService : IItemService
    {
        private readonly IItemRepository _repo;

        public ItemService(IItemRepository repo)
        {
            _repo = repo;
        }

        public Domain.ItemLister GetAll(Domain.ItemLister mLister)
        {
            return _repo.GetAll(mLister);
        }

        public Domain.Item Upsert(Domain.Item mItem)
        {
            return _repo.Upsert(mItem);
        }

        public Domain.Item Get(int id)
        {
            return _repo.Get(id);
        }

        public void Delete(int id, int loginUserId)
        {
            _repo.Delete(id, loginUserId);
        }

        public List<Domain.Item> GetActiveItem()
        {
            return _repo.GetActiveItem();
        }
    }
}
