using CRMWebService.Infrastructure.CustomException;
using CRMWebService.Infrastructure.IRepository;
using CRMWebService.Infrastructure.IService;
using CRMWebService.Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace CRMWebService.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;
        private readonly IFileReaderService _fileReaderService;
        private readonly IEmailSenderService _emailSenderService;
        private readonly IClientRepository _clientRepo;

        public UserService(IUserRepository repo, IEmailSenderService emailSenderService, IFileReaderService fileReaderService, IClientRepository clientRepo)
        {
            _repo = repo;
            _emailSenderService = emailSenderService;
            _fileReaderService = fileReaderService;
            _clientRepo = clientRepo;
        }

        public Domain.UserLister GetAll(Domain.UserLister mLister)
        {
            return _repo.GetAll(mLister);
        }

        public Domain.User Upsert(Domain.User mUser)
        {
            if (mUser == null)
                throw new APIRequestFailedException("User object must not be null.");

            if (mUser.Id == 0)
            {
                mUser = CreateUser(mUser);
            }
            else
            {
                mUser = _repo.Upsert(mUser);
            }

            return mUser;
        }

        public Domain.User Get(int id)
        {
            return _repo.Get(id);
        }

        public void Delete(int id, int loginUserId)
        {
            _repo.Delete(id, loginUserId);
        }

        public bool CheckEmailAddressExist(Domain.User mUser)
        {
            return _repo.CheckEmailAddressExist(mUser);
        }

        public List<Domain.User> GetAllActiveUsersBy(int userTypeId)
        {
            return _repo.GetAllActiveUsersBy(userTypeId);
        }

        #region Private Method

        private void SendEmailOnCreateUser(Domain.User mUser)
        {
            string content = _fileReaderService.ReadFileContent("~/EmailTemplates/SendCredentialsOnCreateUser.html");
            string finalContent = PopulateEmailTemplate(content, mUser);
            _emailSenderService.SendEmail(finalContent, mUser.EmailAddress);
        }

        private string PopulateEmailTemplate(string template, Domain.User mUser)
        {
            var replacements = new Dictionary<string, string>
            {
                { "{{FirstName}}", mUser.FirstName },
                { "{{Username}}", mUser.EmailAddress },
                { "{{Password}}", mUser.PlainTextPassword},
                { "{{Year}}", DateTime.Now.Year.ToString() },
            };

            foreach (var pair in replacements)
            {
                template = template.Replace(pair.Key, pair.Value);
            }

            return template;
        }

        private void SetUserPassword(Domain.User user)
        {
            user.PlainTextPassword = StaticMethods.GeneratePassword(8);
            var salt = ConfigurationManager.AppSettings.Get("Salt");
            user.Password = PasswordEncryptor.Encrypt(user.PlainTextPassword, salt);
        }

        private bool IsValidUser(Domain.User mUser)
        {
            return mUser != null && mUser.Id > 0;
        }

        private Domain.User CreateUser(Domain.User mUser)
        {
            SetUserPassword(mUser);

            mUser = _repo.Upsert(mUser);

            if (IsValidUser(mUser))
            {
                if (mUser.UserTypeId == (int)Enums.UserType.Client)
                {
                    var mClient = PrepareClientModel(mUser.Id, mUser);
                    _clientRepo.Upsert(mClient);
                }
                SendEmailOnCreateUser(mUser);
            }

            return mUser;
        }

        private Domain.Client PrepareClientModel(int clientId, Domain.User mUser)
        {
            var mClient = new Domain.Client
            {
                DyeingSupervisorId = _repo.GetActiveUserBy((int)Enums.UserType.Dyeing_Supervisor).Id,
                CollectionAgentId = _repo.GetActiveUserBy((int)Enums.UserType.Collection_Agent).Id,
                ClientTypeId = 1,
                Brand = mUser.Brand,
                Name = mUser.FirstName,
                ContactPerson = $"{mUser.FirstName} {mUser.LastName}",
                EmailAddress = mUser.EmailAddress,
                MobileNumber = mUser.MobileNumber,
                GSTNumber = string.Empty,
                PANNumber = string.Empty,
                AadharNumber = string.Empty,
                TANNumber = string.Empty,
                CreditLimit = 1000000,
                PaymentTerms = "Net 30 Days",
                //CreatedBy = mUser.CreatedBy,
                //LastModifiedBy = mUser.LastModifiedBy,
                IsActive = true
            };

            return mClient;
        }

        #endregion
    }
}
