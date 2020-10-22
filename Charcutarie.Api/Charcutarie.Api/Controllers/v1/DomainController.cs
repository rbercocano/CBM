using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class DomainController : AuthBaseController
    {
        private readonly IDomainService service;

        public DomainController(IDomainService service)
        {
            this.service = service;
        }
        [HttpGet("MeasureUnit")]
        public async Task<ActionResult<IEnumerable<MeasureUnit>>> GetAllMeasureUnits()
        {
            var data = await service.GetAllMeasureUnits();
            if (data.Any())
                return Ok(data);
            return NoContent();
        }
        [HttpGet("CustomerType")]
        public async Task<ActionResult<IEnumerable<CustomerType>>> GetAllCustomerTypes()
        {
            var data = await service.GetAllCustomerTypes();
            if (data.Any())
                return Ok(data);
            return NoContent();
        }
        [HttpGet("Role")]
        public async Task<ActionResult<IEnumerable<Role>>> GetAllRoles()
        {
            var data = await service.GetAllRoles();
            if (data.Any())
                return Ok(data);
            return NoContent();
        }
        [HttpGet("ContactType")]
        public async Task<ActionResult<IEnumerable<ContactType>>> GetAllContactTypes()
        {
            var data = await service.GetAllContactTypes();
            if (data.Any())
                return Ok(data);
            return NoContent();
        }
        [HttpGet("OrderStatus")]
        public async Task<ActionResult<IEnumerable<ContactType>>> GetAllOrderStatus()
        {
            var data = await service.GetAllOrderStatus();
            if (data.Any())
                return Ok(data);
            return NoContent();
        }
        [HttpGet("OrderItemStatus")]
        public async Task<ActionResult<IEnumerable<ContactType>>> GetAllOrderItemStatus()
        {
            var data = await service.GetAllOrderItemStatus();
            if (data.Any())
                return Ok(data);
            return NoContent();
        }
        [HttpGet("PaymentStatus")]
        public async Task<ActionResult<IEnumerable<ContactType>>> GetAllPaymentStatus()
        {
            var data = await service.GetAllPaymentStatus();
            if (data.Any())
                return Ok(data);
            return NoContent();
        }
        [HttpGet("TransactionType")]
        public async Task<ActionResult<IEnumerable<TransactionType>>> GetAllTransactionTypes()
        {
            var data = await service.GetAllTransactionTypes();
            if (data.Any())
                return Ok(data);
            return NoContent();
        }
    }
}