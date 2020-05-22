using Charcutarie.Application.Contracts;
using Charcutarie.Models;
using Charcutarie.Models.ViewModels;
using Charcutarie.Services.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Charcutarie.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IPersonCustomerApp personCustomerApp;
        private readonly ICompanyCustomerApp companyCustomerApp;
        private readonly ICustomerContactApp customerContactApp;

        public CustomerService(IPersonCustomerApp personCustomerApp, ICompanyCustomerApp companyCustomerApp, ICustomerContactApp customerContactApp)
        {
            this.personCustomerApp = personCustomerApp;
            this.companyCustomerApp = companyCustomerApp;
            this.customerContactApp = customerContactApp;
        }
        public async Task<IEnumerable<MergedCustomer>> FilterCustomers(string filter)
        {
            var tPeople = personCustomerApp.Filter(filter);
            var tCompanies = companyCustomerApp.Filter(filter);
            var people = await tPeople;
            var companies = await tCompanies;

            var result = people.Select(p => new MergedCustomer
            {
                Name = $"{p.Name} {p.LastName}",
                CustomerId = p.CustomerId,
                CustomerTypeId = p.CustomerTypeId,
                CustomerType = p.CustomerType,
                SocialIdentifier = p.Cpf
            }).Union(companies.Select(p => new MergedCustomer
            {
                Name = p.DBAName,
                CustomerId = p.CustomerId,
                CustomerTypeId = p.CustomerTypeId,
                CustomerType = p.CustomerType,
                SocialIdentifier = p.Cnpj
            })).ToList();
            return result;
        }
        public async Task<PagedResult<T>> GetPaged<T>(int page, int pageSize, string filter, int customerTypeId) where T : Customer
        {
            if (customerTypeId == 1)
            {
                var data = await personCustomerApp.GetPaged(page, pageSize, filter);
                return data.As<T>();
            }
            else
            {
                var data = await companyCustomerApp.GetPaged(page, pageSize, filter);
                return data.As<T>();
            }
        }
        public async Task<long> Add(Customer model)
        {
            if (model.CustomerTypeId == 1)
            {
                var newModel = model as NewPersonCustomer;
                return await personCustomerApp.Add(newModel);
            }
            else
            {
                var newModel = model as NewCompanyCustomer;
                return await companyCustomerApp.Add(newModel);
            }
        }

        public async Task<Customer> Update(Customer model)
        {
            if (model.CustomerTypeId == 1)
            {
                var updateModel = model as UpdatePersonCustomer;
                var data = await personCustomerApp.Update(updateModel);
                return (data as Customer);
            }
            else
            {
                var updateModel = model as UpdateCompanyCustomer;
                var data = await companyCustomerApp.Update(updateModel);
                return (data as Customer);
            }
        }
        public async Task<Customer> Get(int id, int customerTypeId)
        {
            if (customerTypeId == 1)
            {
                var data = await personCustomerApp.Get(id);
                return (data as Customer);
            }
            else
            {
                var data = await companyCustomerApp.Get(id);
                return (data as Customer);
            }
        }

        public async Task<long> AddContact(AddCustomerContact model)
        {
            return await customerContactApp.Add(model);
        }

        public async Task DeleteContact(long contactId, int corpClientId)
        {
            await customerContactApp.Delete(contactId, corpClientId);
        }

        public async Task<IEnumerable<CustomerContact>> GetAllContacts(long customerId, int corpClientId)
        {
            return await customerContactApp.GetAll(customerId, corpClientId);
        }

        public async Task UpdateContact(UpdateCustomerContact model, int corpClientId)
        {
            await customerContactApp.Update(model, corpClientId);
        }
    }
}
