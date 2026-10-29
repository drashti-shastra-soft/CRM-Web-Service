using CRMWebService.Infrastructure.IRepository;
using CRMWebService.Infrastructure.IService;
using System.Collections.Generic;

namespace CRMWebService.Service
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _repo;
        private readonly IUserService _userService;

        public ClientService(IClientRepository repo, IUserService userService)
        {
            _repo = repo;
            _userService = userService;
        }

        public Domain.ClientLister GetAll(Domain.ClientLister mLister)
        {
            return _repo.GetAll(mLister);
        }

        public Domain.Client Upsert(Domain.Client mClient)
        {
            var mUser = _userService.Get(mClient.Id);
            return _repo.Upsert(mClient);
        }

        public Domain.Client Get(int id)
        {
            return _repo.Get(id);
        }

        public void Delete(int id, int loginUserId)
        {
            _repo.Delete(id, loginUserId);
        }

        public List<Domain.Client> GetActiveClientTypes()
        {
            return _repo.GetActiveClientTypes();
        }

        public List<Domain.Client> GetActiveClient()
        {
            return _repo.GetActiveClient();
        }


        private bool IsValidUser(Domain.User savedUser)
        {
            return savedUser != null && savedUser.Id > 0;
        }
    }
}
