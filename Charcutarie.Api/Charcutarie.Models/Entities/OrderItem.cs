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
        public double  Quantity{ get; set; }
        public OrderItemStatusEnum OrderItemStatusId { get; set; }
        public MeasureUnitEnum MeasureUnitId { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime LastStatusDate { get; set; }
        
        public DateTime? LastUpdated { get; set; }
        public string AdditionalInfo { get; set; }
        public double OriginalPrice { get; set; }
        public double Discount { get; set; }
        public double PriceAfterDiscount { get; set; }
        public double ProductPrice { get; set; }
        public double? Cost { get; set; }
        public double? Profit { get; set; }
        public Order Order { get; set; }
        public OrderItemStatus OrderItemStatus { get; set; }
        public MeasureUnit MeasureUnit { get; set; }
    }
}
