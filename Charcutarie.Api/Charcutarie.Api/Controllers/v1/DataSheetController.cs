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
    public class DataSheetController : AuthBaseController
    {
        private readonly IDataSheetService service;

        public DataSheetController(IDataSheetService service)
        {
            this.service = service;
        }
        [HttpGet("{id:long}")]
        public async Task<ActionResult<DataSheet>> Get(long id)
        {
            var data = await service.GetDataSheet(id, UserData.CorpClientId.Value);
            if (data != null)
                return Ok(data);
            return NoContent();
        }
        [HttpGet("Item/{id:long}")]
        public async Task<ActionResult<DataSheet>> GetDataSheetItem(long id)
        {
            var data = await service.GetDataSheetItem(id, UserData.CorpClientId.Value);
            if (data != null)
                return Ok(data);
            return NoContent();
        }
        [HttpPost]
        public async Task<ActionResult<long>> Create(DataSheet dataSheet)
        {
            var data = await service.Create(dataSheet, UserData.CorpClientId.Value);
            if (data != null)
                return Ok(data);
            return NoContent();
        }
        [HttpPut]
        public async Task<ActionResult<long>> Update(SaveDataSheet saveDataSheet)
        {
            var data = await service.Update(saveDataSheet, UserData.CorpClientId.Value);
            if (data != null)
                return Ok(data);
            return NoContent();
        }
        [HttpPost("Item")]
        public async Task<ActionResult<long>> AddItem(NewDataSheetItem item)
        {
            var data = await service.AddItem(item, UserData.CorpClientId.Value);
            if (data != null)
                return Ok(data);
            return NoContent();
        }
        [HttpPut("Item")]
        public async Task<ActionResult> UpdateItem(UpdateDataSheetItem item)
        {
            await service.UpdateItem(item, UserData.CorpClientId.Value);
            return Ok();
        }
        [HttpDelete("Item")]
        public async Task<ActionResult> DeleteItem(long itemId)
        {
            await service.DeleteItem(itemId, UserData.CorpClientId.Value);
            return Ok();
        }
    }

}