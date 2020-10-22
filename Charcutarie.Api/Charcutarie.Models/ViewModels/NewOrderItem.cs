using Charcutarie.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Charcutarie.Models.ViewModels
{
    public class NewOrderItem
    {
        public long ProductId { get; set; }
        public decimal Quantity { get; set; }
        public OrderItemStatusEnum OrderItemStatusId { get; set; }
        public string AdditionalInfo { get; set; }
        public decimal Discount { get; set; }
        public decimal? Cost { get; set; }
        public decimal? Profit { get; set; }
        public decimal? ProfitPercentage
        {
            get
            {
                if (Profit.HasValue)
                    return Profit.Value / Cost.Value * 100;
                return null;
            }
        }
        public MeasureUnitEnum MeasureUnitId { get; set; }
        public long OrderId { get; set; }

        [JsonIgnore]
        public int ItemNumber { get; set; }
        [JsonIgnore]
        public decimal OriginalPrice { get; set; }
        [JsonIgnore]
        public decimal PriceAfterDiscount { get { return OriginalPrice - Discount; } }
        [JsonIgnore]
        public decimal ProductPrice { get; set; }
    }
}
