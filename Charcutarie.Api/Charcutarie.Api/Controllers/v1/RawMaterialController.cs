using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Charcutarie.Models;
using Charcutarie.Models.Enums.OrderBy;
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
    public class RawMaterialController : AuthBaseController
    {
        private readonly IRawMaterialService service;

        public RawMaterialController(IRawMaterialService service)
        {
            this.service = service;
        }

        [HttpGet("{page:int}/{pageSize:int}")]
        public async Task<ActionResult<PagedResult<RawMaterial>>> GetPaged(int page, int pageSize, [FromQuery] string? name = "", [FromQuery] OrderByDirection direction = OrderByDirection.Asc)
        {
            var data = await service.GetPaged(UserData.CorpClientId.Value, page, pageSize, name, direction);
            if (data.Data.Any())
                return Ok(data);
            return NoContent();
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<RawMaterial>> Get(int id)
        {
            var data = await service.Get(UserData.CorpClientId.Value, id);
            if (data != null)
                return Ok(data);
            return NoContent();
        }
        [HttpGet()]
        public async Task<ActionResult<IEnumerable<RawMaterial>>> GetAll([FromQuery] OrderByDirection direction = OrderByDirection.Asc)
        {
            var data = await service.GetAll(UserData.CorpClientId.Value, direction);
            if (data != null)
                return Ok(data);
            return NoContent();
        }
        [HttpPost]
        public async Task<ActionResult<long>> Add(NewRawMaterial model)
        {
            model.CorpClientId = UserData.CorpClientId.Value;
            var id = await service.Add(model);
            if (id > 0)
                return Ok(id);
            return new StatusCodeResult(304);
        }
        [HttpPut]
        public async Task<ActionResult<RawMaterial>> Update(UpdateRawmaterial model)
        {
            model.CorpClientId = UserData.CorpClientId.Value;
            var data = await service.Update(model);
            if (data != null)
                return Ok(data);
            return new StatusCodeResult(304);
        }
    }

}