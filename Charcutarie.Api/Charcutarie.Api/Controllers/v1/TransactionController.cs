using System;
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
    public class TransactionController : AuthBaseController
    {
        private readonly ITransactionService service;

        public TransactionController(ITransactionService service)
        {
            this.service = service;
        }

        [HttpGet("Balance")]
        public ActionResult<IEnumerable<Balance>> GetBalance([FromQuery] DateTime start, [FromQuery] DateTime end)
        {
            var data = service.GetBalance(start, end, UserData.CorpClientId.Value);
            if (data.Any())
                return Ok(data);
            return NoContent();
        }
        [HttpGet("Balance/Total")]
        public ActionResult<decimal> GetTotalBalance()
        {
            var data = service.GetTotalBalance(UserData.CorpClientId.Value);
            return Ok(data);
        }

        [HttpPost()]
        public async Task<ActionResult<Transaction>> AddTransaction(NewTransaction transaction)
        {
            transaction.CorpClientId = UserData.CorpClientId.Value;
            transaction.TransactionStatusId = 1;
            var result = await service.AddTransaction(transaction, UserData.UserId);
            return Ok(result);
        }
    }
}