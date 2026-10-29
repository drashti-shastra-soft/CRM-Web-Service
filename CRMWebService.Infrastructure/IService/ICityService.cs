using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMWebService.Infrastructure.IService
{
    public interface ICityService
    {
        Domain.CityLister GetAll(Domain.CityLister mLister);

        Domain.City Upsert(Domain.City mCity);

        Domain.City Get(int id);

        void Delete(int id, int loginUserId);

        List<Domain.City> GetActiveBy(int stateId);

        List<Domain.City> GetActiveBy(string state);
    }
}
