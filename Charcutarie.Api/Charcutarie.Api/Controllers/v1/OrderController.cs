using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Charcutarie.Models;
using Charcutarie.Models.Enums;
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
    public class OrderController : AuthBaseController
    {
        private readonly IOrderService service;

        public OrderController(IOrderService service)
        {
            this.service = service;
        }
        [HttpPost]
        public async Task<ActionResult<int>> Add(NewOrder model)
        {
            var id = await service.Add(model, UserData.CorpClientId.Value);
            if (id > 0)
                return Ok(id);
            return new StatusCodeResult(304);
        }
        [HttpPut]
        public async Task<ActionResult> Update(UpdateOrder model)
        {
            await service.Update(model, UserData.CorpClientId.Value);
            return Ok();
        }
        [HttpPost("Cancel/{orderNumber:int}")]
        public async Task<ActionResult> CancelOrder(int orderNumber)
        {
            await service.ChangeStatus(new UpdateOrderStatus
            {
                OrderStatusId = OrderStatusEnum.Cancelado,
                OrderNumber = orderNumber
            }, UserData.CorpClientId.Value);
            return Ok();
        }
        [HttpPost("Restore/{orderNumber:int}")]
        public async Task<ActionResult<OrderStatusEnum>> Restore(int orderNumber)
        {
            var nextStatus = await service.RestoreOrderStatus(orderNumber, UserData.CorpClientId.Value);
            return Ok(nextStatus);
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
            if (data != null)
                return Ok(data);
            return NoContent();
        }
        [HttpDelete("{orderId:long}/Item/{orderItemId:long}")]
        public async Task<ActionResult> RemoveItem(long orderId, long orderItemId)
        {
            await service.RemoveOrderItem(orderId, orderItemId, UserData.CorpClientId.Value);
            return Ok();
        }
        [HttpPut("Item")]
        public async Task<ActionResult> UpdateItem(UpdateOrderItem model)
        {
            await service.UpdateOrderItem(model, UserData.CorpClientId.Value);
            return Ok();
        }
        [HttpPost("Item")]
        public async Task<ActionResult<long>> AddItem(NewOrderItem model)
        {
            var id = await service.AddOrderItem(model, UserData.CorpClientId.Value);
            return Ok(id);
        }
        [HttpGet("{page:int}/{pageSize:int}")]
        public ActionResult<PagedResult<OrderSummary>> GetOrderSummary([FromQuery] string? customer, [FromQuery] DateTime? createdOnFrom, [FromQuery] DateTime? createdOnTo, [FromQuery] DateTime? paidOnFrom, [FromQuery] DateTime? paidOnTo, [FromQuery] DateTime? completeByFrom, [FromQuery] DateTime? completeByTo, [FromQuery] int? paymentStatus, [FromQuery] List<int> orderStatus, [FromQuery] OrderSummaryOrderBy orderBy = OrderSummaryOrderBy.CreatedOn, [FromQuery] OrderByDirection direction = OrderByDirection.Desc, int? page = 1, int? pageSize = 10)
        {
            var data = service.GetOrderSummary(UserData.CorpClientId.Value, customer, createdOnFrom, createdOnTo, paidOnFrom, paidOnTo, completeByFrom, completeByTo, paymentStatus, orderStatus, orderBy, direction, page, pageSize);
            if (data.Data.Any())
                return Ok(data);
            return NoContent();
        }
    }
}