using System;
using System.Collections.Generic;
using System.Linq;

namespace CRMWebService.Infrastructure.IService
{
    public interface IUserService
    {
        Domain.UserLister GetAll(Domain.UserLister mLister);

        Domain.User Upsert(Domain.User mUser);

        Domain.User Get(int id);

        void Delete(int id, int loginUserId);

        List<Domain.User> GetAllActiveUsersBy(int userTypeId);

        bool CheckEmailAddressExist(Domain.User mUser);

    }
}
