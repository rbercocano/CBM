using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Charcutarie.Models.ViewModels
{
    public class UpdateOrderItem
    {
        public long ProductId { get; set; }
        public double Quantity { get; set; }
        public int OrderItemStatusId { get; set; }
        public string AdditionalInfo { get; set; }
        public double Discount { get; set; }
        public int MeasureUnitId { get; set; }
        public long OrderItemId { get; set; }
        public long OrderId { get; set; }
        [JsonIgnore]
        public double OriginalPrice { get; set; }
        [JsonIgnore]
        public double PriceAfterDiscount { get { return OriginalPrice - Discount; } }
        [JsonIgnore]
        public double ProductPrice { get; set; }
    }
}
