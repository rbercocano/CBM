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
        public int OrderItemStatusId { get; set; }
        public int MeasureUnitId { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? LastUpdated { get; set; }
        public string AdditionalInfo { get; set; }
        public double OriginalPrice { get; set; }
        public double Discount { get; set; }
        public double PriceAfterDiscount { get; set; }
        public double ProductPrice { get; set; }
        public Order Order { get; set; }
        public OrderItemStatus OrderItemStatus { get; set; }
        public MeasureUnit MeasureUnit { get; set; }
    }
}
