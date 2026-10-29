using System;
using System.Collections.Generic;
using System.Linq;

namespace CRMWebService.Infrastructure.IRepository
{
    public interface IUserRepository
    {
        Domain.UserLister GetAll(Domain.UserLister mLister);

        Domain.User Upsert(Domain.User mUser);

        Domain.User Get(int id);

        void Delete(int id, int loginUserId);

        Domain.User GetBy(string emailAddress);

        List<Domain.User> GetAllActiveUsersBy(int userTypeId);

        string GetPasswordBy(int id);

        void SetPasswordBy(int id, string password);

        bool CheckEmailAddressExist(Domain.User mUser);

        Domain.User GetActiveUserBy(int userTypeId);
    }
}
