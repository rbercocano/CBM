using System.Collections.Generic;
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
    public class ProductController : AuthBaseController
    {
        private readonly IProductService service;

        public ProductController(IProductService service)
        {
            this.service = service;
        }

        [HttpGet("{page:int}/{pageSize:int}")]
        public async Task<ActionResult<PagedResult<Product>>> GetPaged(int page, int pageSize, [FromQuery] string? filter = "", [FromQuery] bool? active = null)
        {
            var data = await service.GetPaged(UserData.CorpClientId.Value, page, pageSize, filter, active);
            if (data.Data.Any())
                return Ok(data);
            return NoContent();
        }
        [HttpPost]
        public async Task<ActionResult<long>> Add(NewProduct model)
        {
            model.CorpClientId = UserData.CorpClientId.Value;
            var id = await service.Add(model);
            if (id > 0)
                return Ok(id);
            return new StatusCodeResult(304);
        }
        [HttpPut]
        public async Task<ActionResult<Product>> Update(UpdateProduct model)
        {
            model.CorpClientId = UserData.CorpClientId.Value;
            var data = await service.Update(model);
            if (data != null)
                return Ok(data);
            return new StatusCodeResult(304);
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Product>> Get(int id)
        {
            var data = await service.Get(UserData.CorpClientId.Value, id);
            if (data != null)
                return Ok(data);
            return NoContent();
        }
        [HttpGet()]
        public async Task<ActionResult<IEnumerable<Product>>> GetAll()
        {
            var data = await service.GetAll(UserData.CorpClientId.Value);
            if (data.Any())
                return Ok(data);
            return NoContent();
        }
        [HttpGet("CostAndProfit")]
        public async Task<ActionResult<IEnumerable<ProductionCostProfit>>> GetProductionCostProfit()
        {
            var data = await service.GetProductionCostProfit(UserData.CorpClientId.Value);
            if (data.Any())
                return Ok(data);
            return NoContent();
        }
        [HttpGet("Production")]
        public async Task<ActionResult<IEnumerable<Production>>> GetProduction()
        {
            var data = await service.GetProduction(UserData.CorpClientId.Value);
            if (data.Any())
                return Ok(data);
            return NoContent();
        }
    }

}