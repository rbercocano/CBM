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
    public class CorpClientController : AuthBaseController
    {
        private readonly ICorpClientService service;

        public CorpClientController(ICorpClientService service)
        {
            this.service = service;
        }

        [HttpGet("{page:int}/{pageSize:int}")]
        public async Task<ActionResult<PagedResult<CorpClient>>> GetPaged(int page, int pageSize, [FromQuery] string filter = null, [FromQuery] bool? active = null)
        {
            var data = await service.GetPaged(page, pageSize, filter, active);
            if (data.Data.Any())
                return Ok(data);
            return NoContent();
        }
        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<ActionResult<CorpClient>> Register(ClientRegistration model)
        {
            var data = await service.Register(model);
            return Ok(data);
        }
        [HttpPut]
        public async Task<ActionResult<CorpClient>> Update(UpdateCorpClient model)
        {
            var data = await service.Update(model);
            if (data != null)
                return Ok(data);
            return new StatusCodeResult(304);
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<CorpClient>> Get(int id)
        {
            var data = await service.Get(id);
            if (data != null)
                return Ok(data);
            return NoContent();
        }
        [HttpGet("Actives")]
        [AllowAnonymous]
        public async Task<ActionResult<CorpClient>> GetActives()
        {
            var data = await service.GetActives();
            if (data != null)
                return Ok(data);
            return NoContent();
        }
    }
}