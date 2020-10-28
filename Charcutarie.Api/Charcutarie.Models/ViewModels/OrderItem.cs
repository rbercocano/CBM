using Charcutarie.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Charcutarie.Models.ViewModels
{
    public class OrderItem
    {
        public Product Product { get; set; }
        public MeasureUnit MeasureUnit { get; set; }
        public OrderItemStatus OrderItemStatus { get; set; }
        public OrderItemStatusEnum OrderItemStatusId { get; set; }
        public decimal Quantity { get; set; }
        public string AdditionalInfo { get; set; }
        public decimal Discount { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal PriceAfterDiscount { get; set; }
        public decimal ProductPrice { get; set; }
        public decimal? Cost { get; set; }
        public decimal? Profit { get; set; }
        public decimal? ProfitPercentage { get; set; }
        public int ItemNumber { get; set; }
        public int OrderItemId { get; set; }
        public Order Order { get; set; }
        public DateTimeOffset LastStatusDate { get; set; }
    }
}
