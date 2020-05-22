using System.Linq;
using System.Threading.Tasks;
using Charcutarie.Models;
using Charcutarie.Models.ViewModels;
using Charcutarie.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Charcutarie.Api.Controllers.v1
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize(Policy = "ApiAccess", Roles = "Customer,SysAdmin")]
    [ApiController]
    public class CustomerController : AuthBaseController
    {
        private readonly ICustomerService service;

        public CustomerController(ICustomerService service)
        {
            this.service = service;
        }

        [HttpGet("Person/{page:int}/{pageSize:int}")]
        public async Task<ActionResult<PagedResult<PersonCustomer>>> GetPagedPerson(int page, int pageSize, [FromQuery]string? filter = null)
        {
            var data = await service.GetPaged<PersonCustomer>(page, pageSize, filter, 1);
            if (data.Data.Any())
                return Ok(data);
            return NoContent();
        }
        [HttpPost("Person")]
        public async Task<ActionResult<long>> AddPerson(NewPersonCustomer model)
        {
            model.CorpClientId = UserData.CorpClientId.Value;
            model.CustomerTypeId = 1;
            var id = await service.Add(model);
            if (id > 0)
                return Ok(id);
            return new StatusCodeResult(304);
        }
        [HttpPut("Person")]
        public async Task<ActionResult<PersonCustomer>> UpdatePerson(UpdatePersonCustomer model)
        {
            model.CustomerTypeId = 1;
            var data = await service.Update(model);
            if (data != null)
                return Ok(data);
            return new StatusCodeResult(304);
        }
        [HttpGet("Person/{id:int}")]
        public async Task<ActionResult<Customer>> GetPerson(int id)
        {
            var data = await service.Get(id, 1);
            if (data != null)
                return Ok(data);
            return NoContent();
        }


        [HttpGet("Company/{page:int}/{pageSize:int}")]
        public async Task<ActionResult<PagedResult<CompanyCustomer>>> GetPagedCompany(int page, int pageSize, [FromQuery]string? filter = null)
        {
            var data = await service.GetPaged<CompanyCustomer>(page, pageSize, filter, 2);
            if (data.Data.Any())
                return Ok(data);
            return NoContent();
        }
        [HttpPost("Company")]
        public async Task<ActionResult<long>> AddCompany(NewCompanyCustomer model)
        {
            model.CorpClientId = UserData.CorpClientId.Value;
            model.CustomerTypeId = 2;
            var id = await service.Add(model);
            if (id > 0)
                return Ok(id);
            return new StatusCodeResult(304);
        }
        [HttpPut("Company")]
        public async Task<ActionResult<CompanyCustomer>> UpdateCompany(UpdateCompanyCustomer model)
        {
            var data = await service.Update(model);
            model.CustomerTypeId = 2;
            if (data != null)
                return Ok(data);
            return new StatusCodeResult(304);
        }
        [HttpGet("Company/{id:int}")]
        public async Task<ActionResult<Customer>> GetCompany(int id)
        {
            var data = await service.Get(id, 2);
            if (data != null)
                return Ok(data);
            return NoContent();
        }


        [HttpGet("Contact/{id:long}")]
        public async Task<ActionResult<CustomerContact>> GetContacts(long id)
        {
            var data = await service.GetAllContacts(id, UserData.CorpClientId.Value);
            if (data != null)
                return Ok(data);
            return NoContent();
        }

        [HttpPost("Contact")]
        public async Task<ActionResult<long>> AddContact(AddCustomerContact model)
        {
            var data = await service.AddContact(model);
            return Ok(data);
        }
        [HttpPut("Contact")]
        public async Task<ActionResult> UpdateContact(UpdateCustomerContact model)
        {
            await service.UpdateContact(model, UserData.CorpClientId.Value);
            return Ok();
        }
        [HttpDelete("Contact/{id:long}")]
        public async Task<ActionResult> DeleteContact(long id)
        {
            await service.DeleteContact(id, UserData.CorpClientId.Value);
            return Ok();
        }

        [HttpGet("Merged/{filter}")]
        public async Task<ActionResult> Filter(string filter)
        {
            var data = await service.FilterCustomers(filter);
            if (data != null)
                return Ok(data);
            return NoContent();
        }
    }
}