using CRMWebService.Domain;
using CRMWebService.Infrastructure.IRepository;
using CRMWebService.Infrastructure.IService;
using System.Collections.Generic;


namespace CRMWebService.Service
{
    public class UserTypeService : IUserTypeService
    {
        private readonly IUserTypeRepository _repo;

        public UserTypeService(IUserTypeRepository repo)
        {
            _repo = repo;
        }

        public List<UserType> GetAll()
        {
            return _repo.GetAll();
        }
    }
}
