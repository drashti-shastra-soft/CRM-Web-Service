using CRMWebService.Infrastructure.IRepository;
using CRMWebService.Infrastructure.IService;
using CRMWebService.Repository;
using CRMWebService.Service;
using System.Web.Http;
using Unity;
using Unity.WebApi;

namespace CRMWebService
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();

            //Service

            container.RegisterType<IAddressService, AddressService>();
            container.RegisterType<IAuthenticationService, AuthenticationService>();
            container.RegisterType<IBankService, BankService>();
            container.RegisterType<IBrokerService, BrokerService>();
            container.RegisterType<IChangePasswordService, ChangePasswordService>();
            container.RegisterType<ICityService, CityService>();
            container.RegisterType<IClientService, ClientService>();
            container.RegisterType<IClientTypeService, ClientTypeService>();
            container.RegisterType<IEmailSenderService, EmailSenderService>();
            container.RegisterType<IFileReaderService, FileReaderService>();
            container.RegisterType<IForgotPasswordService, ForgotPasswordService>();
            container.RegisterType<IInvoiceLineItemService, InvoiceLineItemService>();
            container.RegisterType<IInvoiceService, InvoiceService>();
            container.RegisterType<IItemService, ItemService>();
            container.RegisterType<IJWTokenService, JWTokenService>();
            container.RegisterType<IMarketService, MarketService>();
            container.RegisterType<IOutstandingService, OutstandingService>();
            container.RegisterType<IPhoneNumberService, PhoneNumberService>();
            container.RegisterType<IReceiptAllocationService, ReceiptAllocationService>();
            container.RegisterType<IReceiptService, ReceiptService>();
            container.RegisterType<ISalesReceiptService, SalesReceiptService>();
            container.RegisterType<ISalesReportService, SalesReportService>();
            container.RegisterType<IStateService, StateService>();
            container.RegisterType<IUserService, UserService>();
            container.RegisterType<IUserTypeService, UserTypeService>();

            //Repository
            container.RegisterType<IAddressRepository, AddressRepository>();
            container.RegisterType<IBankRepository, BankRepository>();
            container.RegisterType<IBrokerRepository, BrokerRepository>();
            container.RegisterType<ICityRepository, CityRepository>();
            container.RegisterType<IClientRepository, ClientRepository>();
            container.RegisterType<IClientTypeRepository, ClientTypeRepository>();
            container.RegisterType<IInvoiceLineItemRepository, InvoiceLineItemRepository>();
            container.RegisterType<IInvoiceRepository, InvoiceRepository>();
            container.RegisterType<IItemRepository, ItemRepository>();
            container.RegisterType<IMarketRepository, MarketRepository>();
            container.RegisterType<IOutstandingRepository, OutstandingRepository>();
            container.RegisterType<IPhoneNumberRepository, PhoneNumberRepository>();
            container.RegisterType<IReceiptAllocationRepository, ReceiptAllocationRepository>();
            container.RegisterType<IReceiptRepository, ReceiptRepository>();
            container.RegisterType<ISalesReceiptRepository, SalesReceiptRepository>();
            container.RegisterType<ISalesReportRepository, SalesReportRepository>();
            container.RegisterType<IStateRepository, StateRepository>();
            container.RegisterType<IUserRepository, UserRepository>();
            container.RegisterType<IUserTypeRepository, UserTypeRepository>();

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}