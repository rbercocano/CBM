using AutoMapper;
using Charcutarie.Models.ViewModels;
using Charcutarie.Repository.Contracts;
using Charcutarie.Repository.DbContext;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ef = Charcutarie.Models.Entities;

namespace Charcutarie.Repository
{
    public class CustomerContactRepository : ICustomerContactRepository
    {
        private readonly CharcuterieDbContext context;
        private readonly IMapper mapper;

        public CustomerContactRepository(CharcuterieDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        public async Task<IEnumerable<CustomerContact>> GetAll(long customerId, int corpClientId)
        {
            var data = await context.CustomerContacts
                .Include(c => c.ContactType)
                .Where(c => c.CustomerId == customerId && c.Customer.CorpClientId == corpClientId).ToListAsync();
            var result = mapper.Map<IEnumerable<CustomerContact>>(data);
            return result;
        }
        public async Task<long> Add(AddCustomerContact model)
        {
            var entity = mapper.Map<Ef.CustomerContact>(model);
            context.CustomerContacts.Add(entity);
            await context.SaveChangesAsync();
            return entity.CustomerContactId;
        }
        public async Task Update(UpdateCustomerContact model, int corpClientId)
        {
            var data = await context.CustomerContacts
                .Where(c => c.CustomerId == model.CustomerId && c.Customer.CorpClientId == corpClientId && c.CustomerContactId == model.CustomerContactId).FirstOrDefaultAsync();
            if (data != null)
            {
                data.ContactTypeId = model.ContactTypeId;
                data.AdditionalInfo = model.AdditionalInfo;
                data.Contact = model.Contact;
                context.Update(data);
                await context.SaveChangesAsync();
            }
        }
        public async Task Delete(long contactId, int corpClientId)
        {
            var data = await context.CustomerContacts
                .Where(c => c.CustomerContactId == contactId && c.Customer.CorpClientId == corpClientId).FirstOrDefaultAsync();
            if (data != null)
            {
                context.CustomerContacts.Remove(data);
                await context.SaveChangesAsync();
            }
        }
    }
}
