using AutoMapper;
using vm = Charcutarie.Models.ViewModels;
using ef = Charcutarie.Models.Entities;

namespace Charcutarie.Models.MappingProfile
{
    public class AutoMapperProfileConfiguration : Profile
    {
        public AutoMapperProfileConfiguration() : this("CharcutarieProfile")
        {
        }
        public AutoMapperProfileConfiguration(string profileName) : base(profileName)
        {
            CreateMap<ef.MeasureUnit, vm.MeasureUnit>().ReverseMap();
            CreateMap<ef.CustomerType, vm.CustomerType>().ReverseMap();
            CreateMap<ef.Role, vm.Role>().ReverseMap();

            CreateMap<ef.CorpClient, vm.CorpClient>().ReverseMap();
            CreateMap<ef.CorpClient, vm.NewCorpClient>().ReverseMap();
            CreateMap<ef.CorpClient, vm.UpdateCorpClient>().ReverseMap();

            CreateMap<ef.Product, vm.Product>()
                .ForMember(m => m.MeasureUnit, m => m.MapFrom(s => s.MeasureUnit.Description));
            CreateMap<ef.Product, vm.NewProduct>().ReverseMap();
            CreateMap<ef.Product, vm.UpdateProduct>().ReverseMap();

            CreateMap<ef.PersonCustomer, vm.UpdatePersonCustomer>().ReverseMap();
            CreateMap<ef.PersonCustomer, vm.NewPersonCustomer>().ReverseMap();

            CreateMap<ef.CompanyCustomer, vm.UpdateCompanyCustomer>().ReverseMap();
            CreateMap<ef.CompanyCustomer, vm.NewCompanyCustomer>().ReverseMap();

            CreateMap<ef.PersonCustomer, vm.PersonCustomer>()
                .ForMember(m => m.CustomerType, m => m.MapFrom(s => s.CustomerType.Description));
            CreateMap<ef.CompanyCustomer, vm.CompanyCustomer>()
                .ForMember(m => m.CustomerType, m => m.MapFrom(s => s.CustomerType.Description));

            CreateMap<ef.User, vm.NewUser>().ReverseMap();
            CreateMap<ef.User, vm.User>().ReverseMap();
            CreateMap<ef.User, vm.UpdateUser>().ReverseMap();
            CreateMap<ef.User, vm.JWTUserInfo>()
                .ForMember(m => m.CompanyName, opt => opt.MapFrom(m => m.CorpClient == null ? "" : m.CorpClient.Name))
                .ForMember(m => m.Role, opt => opt.MapFrom(m => m.Role.Name))
                .ForMember(m => m.DBAName, opt => opt.MapFrom(m => m.CorpClient == null ? "" : m.CorpClient.DBAName))
                .ForMember(m => m.Currency, opt => opt.MapFrom(m => m.CorpClient == null ? "" : m.CorpClient.Currency));


            CreateMap<ef.ContactType, vm.ContactType>().ReverseMap();
            CreateMap<ef.CustomerContact, vm.CustomerContact>()
                .ForMember(m => m.ContactType, opt => opt.MapFrom(m => m.ContactType == null ? "" : m.ContactType.Description))
                .ForMember(m => m.ContactIcon, opt => opt.MapFrom(m => m.ContactType == null ? "" : m.ContactType.Icon));
            CreateMap<ef.CustomerContact, vm.UpdateCustomerContact>().ReverseMap();
            CreateMap<ef.CustomerContact, vm.AddCustomerContact>().ReverseMap();

            CreateMap<ef.OrderStatus, vm.OrderStatus>().ReverseMap();
            CreateMap<ef.OrderItemStatus, vm.OrderItemStatus>().ReverseMap();
            CreateMap<ef.PaymentStatus, vm.PaymentStatus>().ReverseMap();

            CreateMap<ef.Order, vm.NewOrder>().ReverseMap();
            CreateMap<ef.OrderItem, vm.NewOrderItem>().ReverseMap();

            CreateMap<ef.Order, vm.Order>().ReverseMap();
            CreateMap<ef.OrderItem, vm.OrderItem>().ReverseMap();




            CreateMap<ef.Customer, vm.MergedCustomer>().ReverseMap();
            CreateMap<ef.PersonCustomer, vm.MergedCustomer>()
                .ForMember(m => m.Name, m => m.MapFrom(p => $"{p.Name} {p.LastName}".Trim()))
                .ForMember(m => m.SocialIdentifier, m => m.MapFrom(p => p.Cpf));

            CreateMap<ef.CompanyCustomer, vm.MergedCustomer>()
                .ForMember(m => m.Name, m => m.MapFrom(p => p.DBAName))
                .ForMember(m => m.SocialIdentifier, m => m.MapFrom(p => p.Cnpj));

            CreateMap<ef.Customer, vm.MergedCustomer>()
                .ForMember(m => m.Name, m => m.MapFrom(s => s is ef.PersonCustomer ? $"{((ef.PersonCustomer)s).Name} {((ef.PersonCustomer)s).LastName}".Trim() : ((ef.CompanyCustomer)s).DBAName))
                .ForMember(m => m.SocialIdentifier, m => m.MapFrom(s => s is ef.PersonCustomer ? ((ef.PersonCustomer)s).Cpf : ((ef.CompanyCustomer)s).Cnpj));
            CreateMap<ef.Customer, vm.UpdatePersonCustomer>().ReverseMap();
            CreateMap<ef.Customer, vm.NewPersonCustomer>().ReverseMap();

            CreateMap<ef.Customer, vm.UpdateCompanyCustomer>().ReverseMap();
            CreateMap<ef.Customer, vm.NewCompanyCustomer>().ReverseMap();

            CreateMap<ef.Customer, vm.PersonCustomer>()
                .ForMember(m => m.CustomerType, m => m.MapFrom(s => s.CustomerType.Description));
            CreateMap<ef.Customer, vm.CompanyCustomer>()
                .ForMember(m => m.CustomerType, m => m.MapFrom(s => s.CustomerType.Description));
            CreateMap<ef.OrderSummary, vm.OrderSummary>().ReverseMap();


            CreateMap<ef.RawMaterial, vm.RawMaterial>()
                .ForMember(m => m.MeasureUnit, m => m.MapFrom(s => s.MeasureUnit.Description));

            CreateMap<ef.RawMaterial, vm.NewRawMaterial>().ReverseMap();
            CreateMap<ef.RawMaterial, vm.UpdateRawmaterial>().ReverseMap();


            CreateMap<ef.DataSheet, vm.DataSheet>().ReverseMap();
            CreateMap<ef.DataSheetItem, vm.DataSheetItem>().ReverseMap();

            CreateMap<ef.DataSheetItem, vm.NewDataSheetItem>().ReverseMap();
            CreateMap<ef.DataSheetItem, vm.UpdateDataSheetItem>().ReverseMap();

            CreateMap<ef.DataSheet, vm.SaveDataSheet>().ReverseMap();
            CreateMap<ef.OrderItemReport, vm.OrderItemReport>().ReverseMap();
            CreateMap<ef.OrderCountSummary, vm.OrderCountSummary>().ReverseMap();
            CreateMap<ef.PendingPaymentsSummary, vm.PendingPaymentsSummary>().ReverseMap();
            CreateMap<ef.ProfitSummary, vm.ProfitSummary>().ReverseMap();
            CreateMap<ef.SalesSummary, vm.SalesSummary>().ReverseMap();
            CreateMap<ef.ProductionCostProfit, vm.ProductionCostProfit>().ReverseMap();
            CreateMap<ef.Production, vm.Production>().ReverseMap();
            CreateMap<ef.SalesPerMonth, vm.SalesPerMonth>().ReverseMap();
            CreateMap<ef.SummarizedOrderReport, vm.SummarizedOrderReport>().ReverseMap();
        }
    }
}
