using CRMWebService.Infrastructure.IRepository;
using CRMWebService.Infrastructure.IService;
using System.Collections.Generic;

namespace CRMWebService.Service
{
    public class CityService : ICityService
    {
        private readonly ICityRepository _repo;

        public CityService(ICityRepository repo)
        {
            _repo = repo;
        }

        public Domain.CityLister GetAll(Domain.CityLister mLister)
        {
            return _repo.GetAll(mLister);
        }

        public Domain.City Upsert(Domain.City mCity)
        {
            return _repo.Upsert(mCity);
        }

        public Domain.City Get(int id)
        {
            return _repo.Get(id);
        }

        public void Delete(int id, int loginUserId)
        {
            _repo.Delete(id, loginUserId);
        }

        public List<Domain.City> GetActiveBy(int stateId)
        {
            return _repo.GetActiveBy(stateId);
        }

        public List<Domain.City> GetActiveBy(string state)
        {
            return _repo.GetActiveBy(state);
        }

    }
}
