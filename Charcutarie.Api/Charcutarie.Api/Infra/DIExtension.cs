﻿using Charcutarie.Application;
using Charcutarie.Application.Contracts;
using Charcutarie.Repository;
using Charcutarie.Repository.Contracts;
using Charcutarie.Services;
using Charcutarie.Services.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace Charcutarie.Api.Infra
{
    public static class DIExtension
    {
        public static void AddDIServices(this IServiceCollection services)
        {
            AddServices(services);
            AddApps(services);
            AddReps(services);
        }
        private static void AddServices(IServiceCollection services)
        {
            services.AddTransient<IDomainService, DomainService>();
            services.AddTransient<ICorpClientService, CorpClientService>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<ICustomerService, CustomerService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ISystemModuleService, SystemModuleService>();
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IPricingService, PricingService>();
            services.AddTransient<IOrderService, OrderService>();
        }
        private static void AddApps(IServiceCollection services)
        {
            services.AddTransient<IMeasureUnitApp, MeasureUnitApp>();
            services.AddTransient<ICustomerTypeApp, CustomerTypeApp>();
            services.AddTransient<ICorpClientApp, CorpClientApp>();
            services.AddTransient<IProductApp, ProductApp>();
            services.AddTransient<IPersonCustomerApp, PersonCustomerApp>();
            services.AddTransient<ICompanyCustomerApp, CompanyCustomerApp>();
            services.AddTransient<IRoleApp, RoleApp>();
            services.AddTransient<IUserApp, UserApp>();
            services.AddTransient<ISystemModuleApp, SystemModuleApp>();
            services.AddTransient<ICustomerContactApp, CustomerContactApp>();
            services.AddTransient<IContactTypeApp, ContactTypeApp>();
            services.AddTransient<IOrderStatusApp, OrderStatusApp>();
            services.AddTransient<IOrderItemStatusApp, OrderItemStatusApp>();
            services.AddTransient<IPaymentStatusApp, PaymentStatusApp>();
            services.AddTransient<IOrderApp, OrderApp>();
            services.AddTransient<IPricingApp, PricingApp>();
        }
        private static void AddReps(IServiceCollection services)
        {
            services.AddTransient<IMeasureUnitRepository, MeasureUnitRepositoy>();
            services.AddTransient<ICustomerTypeRepository, CustomerTypeRepository>();
            services.AddTransient<ICorpClientRepository, CorpClientRepository>();
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<IPersonCustomerRepository, PersonCustomerRepository>();
            services.AddTransient<ICompanyCustomerRepository, CompanyCustomerRepository>();
            services.AddTransient<IRoleRepository, RoleRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<ISystemModuleRepository, SystemModuleRepository>();
            services.AddTransient<ICustomerContactRepository, CustomerContactRepository>();
            services.AddTransient<IContactTypeRepository, ContactTypeRepository>();
            services.AddTransient<IOrderStatusRepository, OrderStatusRepository>();
            services.AddTransient<IOrderItemStatusRepository, OrderItemStatusRepository>();
            services.AddTransient<IPaymentStatusRepository, PaymentStatusRepository>();
            services.AddTransient<IOrderRepository, OrderRepository>();
        }
    }
}
