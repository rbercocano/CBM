using Charcutarie.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Charcutarie.Models.Entities
{
    public class OrderItem
    {
        public long OrderItemId { get; set; }
        public long OrderId { get; set; }
        public int ItemNumber { get; set; }
        public long ProductId { get; set; }
        public Product Product { get; set; }
        public decimal  Quantity{ get; set; }
        public OrderItemStatusEnum OrderItemStatusId { get; set; }
        public MeasureUnitEnum MeasureUnitId { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset LastStatusDate { get; set; }
        
        public DateTimeOffset? LastUpdated { get; set; }
        public string AdditionalInfo { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal PriceAfterDiscount { get; set; }
        public decimal ProductPrice { get; set; }
        public decimal? Cost { get; set; }
        public decimal? Profit { get; set; }
        public decimal? ProfitPercentage { get; set; }
        
        public Order Order { get; set; }
        public OrderItemStatus OrderItemStatus { get; set; }
        public MeasureUnit MeasureUnit { get; set; }
    }
}
