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
    public class OrderController : AuthBaseController
    {
        private readonly IOrderService service;

        public OrderController(IOrderService service)
        {
            this.service = service;
        }
        [HttpPost]
        public async Task<ActionResult<long>> Add(NewOrder model)
        {
            var id = await service.Add(model, UserData.CorpClientId.Value);
            if (id > 0)
                return Ok(id);
            return new StatusCodeResult(304);
        }
        [HttpGet("{id:long}")]
        public async Task<ActionResult<Order>> Get(long id)
        {
            var data = await service.Get(id, UserData.CorpClientId.Value);
            if (id != null)
                return Ok(data);
            return NoContent();
        }
        [HttpGet("number/{number:long}")]
        public async Task<ActionResult<Order>> GetByNumber(int number)
        {
            var data = await service.GetByNumber(number, UserData.CorpClientId.Value);
            if (number != null)
                return Ok(data);
            return NoContent();
        }
    }
}