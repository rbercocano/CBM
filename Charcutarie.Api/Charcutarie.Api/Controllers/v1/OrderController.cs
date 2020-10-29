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
        [HttpPost("Payment")]
        public async Task<ActionResult> Pay(PayOrder model)
        {
            await service.AddPayment(model, UserData.CorpClientId.Value, UserData.UserId);
            return Ok();
        }
        [HttpPost("Refund")]
        public async Task<ActionResult> Refund(RefundPayment model)
        {
            await service.RefundPayment(model, UserData.CorpClientId.Value, UserData.UserId);
            return Ok();
        }
        [HttpPost("Cancel/{orderNumber:int}")]
        public async Task<ActionResult> CancelOrder(long orderNumber)
        {
            await service.ChangeStatus(new UpdateOrderStatus
            {
                OrderStatusId = OrderStatusEnum.Cancelado,
                OrderNumber = orderNumber
            }, UserData.CorpClientId.Value);
            return Ok();
        }
        [HttpPost("Restore/{orderNumber:int}")]
        public async Task<ActionResult<OrderStatusEnum>> Restore(long orderNumber)
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
        public ActionResult<PagedResult<OrderSummary>> GetOrderSummary([FromQuery] string? customer, [FromQuery] DateTimeOffset? createdOnFrom, [FromQuery] DateTimeOffset? createdOnTo, [FromQuery] DateTimeOffset? paidOnFrom, [FromQuery] DateTimeOffset? paidOnTo, [FromQuery] DateTime? completeByFrom, [FromQuery] DateTime? completeByTo, [FromQuery] List<int> paymentStatus, [FromQuery] List<int> orderStatus, [FromQuery] OrderSummaryOrderBy orderBy = OrderSummaryOrderBy.CreatedOn, [FromQuery] OrderByDirection direction = OrderByDirection.Desc, int? page = 1, int? pageSize = 10)
        {
            var data = service.GetOrderSummary(UserData.CorpClientId.Value, customer, createdOnFrom, createdOnTo, paidOnFrom, paidOnTo, completeByFrom, completeByTo, paymentStatus, orderStatus, orderBy, direction, page, pageSize);
            if (data.Data.Any())
                return Ok(data);
            return NoContent();
        }
        [HttpGet("Report/Item/{page:int}/{pageSize:int}")]
        public ActionResult<PagedResult<OrderItemReport>> GetOrderItemReport([FromQuery] int? orderNumber, [FromQuery] List<long> productId, [FromQuery] List<OrderStatusEnum> orderStatus, [FromQuery] List<OrderItemStatusEnum> itemStatus, [FromQuery] DateTime? completeByFrom, [FromQuery] DateTime? completeByTo, [FromQuery] string? customer, [FromQuery] long? customerId, [FromQuery] int massUnitId = 1, [FromQuery] int volumeUnitId = 5, [FromQuery] OrderItemReportOrderBy orderBy = OrderItemReportOrderBy.OrderItemStatus, [FromQuery] OrderByDirection direction = OrderByDirection.Asc, int? page = 1, int? pageSize = 10)
        {
            var data = service.GetOrderItemReport(UserData.CorpClientId.Value, orderNumber, productId, massUnitId, volumeUnitId, orderStatus, itemStatus, completeByFrom, completeByTo, customer, customerId, orderBy, direction, page, pageSize);
            if (data.Data.Any())
                return Ok(data);
            return NoContent();
        }
        [HttpPost("Close/{orderNumber:int}")]
        public async Task<ActionResult> CloseOrder(long orderNumber)
        {
            await service.CloseOrder(orderNumber, UserData.CorpClientId.Value);
            return Ok();
        }

        [HttpGet("Report/OrderCountSummary")]
        public async Task<ActionResult<PagedResult<OrderCountSummary>>> GetOrderCountSummary()
        {
            var data = await service.GetOrderCountSummary(UserData.CorpClientId.Value);
            if (data != null)
                return Ok(data);
            return NoContent();
        }

        [HttpGet("Report/ProfitSummary")]
        public async Task<ActionResult<PagedResult<ProfitSummary>>> GetProfitSummary()
        {
            var data = await service.GetProfitSummary(UserData.CorpClientId.Value);
            if (data != null)
                return Ok(data);
            return NoContent();
        }
        [HttpGet("Report/SalesSummary")]
        public async Task<ActionResult<PagedResult<SalesSummary>>> GetSalesSummary()
        {
            var data = await service.GetSalesSummary(UserData.CorpClientId.Value);
            if (data != null)
                return Ok(data);
            return NoContent();
        }
        [HttpGet("Report/PendingPaymentsSummary")]
        public async Task<ActionResult<PagedResult<PendingPaymentsSummary>>> GetPendingPaymentsSummary()
        {
            var data = await service.GetPendingPaymentsSummary(UserData.CorpClientId.Value);
            if (data != null)
                return Ok(data);
            return NoContent();
        }
        [HttpGet("Report/SalesPerMonth")]
        public async Task<ActionResult<IEnumerable<SalesPerMonth>>> GetSalesPerMonth()
        {
            var data = await service.GetSalesPerMonth(UserData.CorpClientId.Value);
            if (data != null)
                return Ok(data);
            return NoContent();
        }
        [HttpGet("Report/SummarizedProduction/{page:int}/{pageSize:int}")]
        public ActionResult<IEnumerable<SummarizedOrderReport>> GetSummarizedReport([FromQuery] List<OrderItemStatusEnum> itemStatus,
                                                                                        [FromQuery] List<long> products,
                                                                                        [FromQuery] int volumeUnitId = 5, [FromQuery] int massUnitId = 1,
                                                                                        [FromQuery] SummarizedOrderOrderBy orderBy = SummarizedOrderOrderBy.OrderItemStatus,
                                                                                        [FromQuery] OrderByDirection direction = OrderByDirection.Asc, int? page = 1, int? pageSize = 10)
        {
            var data = service.GetSummarizedReport(UserData.CorpClientId.Value, volumeUnitId, massUnitId, itemStatus, products, orderBy, direction, page, pageSize);
            if (data != null)
                return Ok(data);
            return NoContent();
        }
    }
}