using CRMWebService.Infrastructure.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace CRMWebService.Repository
{
    public class UserTypeRepository : IUserTypeRepository
    {
        public List<Domain.UserType> GetAll()
        {
            var mUserType = new List<Domain.UserType>();
            try
            {
                using (var context = new Persistence.CRMDBEntities())
                {
                    var efUserType = (from ut in context.UserTypes
                                      where ut.IsActive
                                      select new
                                      {
                                          Id = ut.Id,
                                          Name = ut.Name,
                                      }).AsEnumerable();

                    mUserType = JsonSerializer.Deserialize<List<Domain.UserType>>(JsonSerializer.Serialize(efUserType.ToList()));
                }
            }
            catch(Exception)
            {
                throw;
            }
            return mUserType;
        }
    }
}
