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
    [Authorize(Policy = "ApiAccess", Roles = "SysAdmin,Customer")]
    [ApiController]
    public class UserController : AuthBaseController
    {
        private readonly IUserService service;

        public UserController(IUserService service)
        {
            this.service = service;
        }

        [HttpGet("{page:int}/{pageSize:int}")]
        public async Task<ActionResult<PagedResult<User>>> GetPaged(int page, int pageSize, [FromQuery] string filter = null, [FromQuery] bool? active = null)
        {
            var data = await service.GetPaged(page, pageSize, filter, active);
            if (data.Data.Any())
                return Ok(data);
            return NoContent();
        }
        [HttpPost]
        public async Task<ActionResult<long>> Add(NewUser model)
        {
            var id = await service.Add(model, UserData.CorpClientId);
            if (id > 0)
                return Ok(id);
            return new StatusCodeResult(304);
        }
        [HttpPut]
        public async Task<ActionResult<User>> Update(UpdateUser model)
        {
            var data = await service.Update(model, UserData.CorpClientId);
            if (data != null)
                return Ok(data);
            return new StatusCodeResult(304);
        }
        [HttpGet("{id:long}")]
        public async Task<ActionResult<User>> Get(long id)
        {
            var data = await service.Get(id, UserData.CorpClientId.Value);
            if (data != null)
                return Ok(data);
            return NoContent();
        }
        [HttpGet("Menus/{userId:int}")]
        public async Task<ActionResult<IEnumerable<ParentModule>>> GetUserModules(long userId)
        {
            var data = await service.GetUserModules(userId);
            if (data != null)
                return Ok(data);
            return NoContent();
        }
        [HttpPost("Password/Reset")]
        [AllowAnonymous]
        public async Task<ActionResult<long>> ResetPassword(PasswordReset data)
        {
            var email = await service.ResetPassword(data.UserName, data.CorpClientId);
            return Ok(email);
        }
    }
}