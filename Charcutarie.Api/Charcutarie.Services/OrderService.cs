using Charcutarie.Application;
using Charcutarie.Application.Contracts;
using Charcutarie.Models;
using Charcutarie.Models.Enums;
using Charcutarie.Models.ViewModels;
using Charcutarie.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Charcutarie.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderApp orderApp;
        private readonly IPricingApp pricingApp;
        private readonly IProductApp productApp;

        public OrderService(IOrderApp orderApp, IPricingApp pricingApp, IProductApp productApp)
        {
            this.orderApp = orderApp;
            this.pricingApp = pricingApp;
            this.productApp = productApp;
        }
        public async Task<long> Add(NewOrder model, int corpClientId)
        {
            var ids = model.OrderItems.Select(i => i.ProductId).ToList();
            var prods = await productApp.GetRange(corpClientId, ids);
            int i = 1;
            foreach (var item in model.OrderItems)
            {
                var prod = prods.FirstOrDefault(p => p.ProductId == item.ProductId);
                item.ProductPrice = prod.Price;
                item.ItemNumber = i;
                item.OriginalPrice = pricingApp.CalculatePrice(new PriceRequest
                {
                    ProductMeasureUnit = (MeasureUnitEnum)prod.MeasureUnitId,
                    ProductPrice = prod.Price,
                    Quantity = item.Quantity,
                    QuantityMeasureUnit = (MeasureUnitEnum)item.MeasureUnitId
                });
                item.ItemNumber++;
            }
            return await orderApp.Add(model, corpClientId);
        }

        public async Task<Order> Get(long orderId, int corpClientId)
        {
            return await orderApp.Get(orderId, corpClientId);
        }

        public async Task<Order> GetByNumber(int orderNumber, int corpClientId)
        {
            return await orderApp.GetByNumber(orderNumber, corpClientId);
        }
    }
}
