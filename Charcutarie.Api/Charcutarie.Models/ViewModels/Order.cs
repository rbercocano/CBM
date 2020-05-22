using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charcutarie.Models.ViewModels
{
    public class Order
    {
        public Order()
        {
            OrderItems = new List<OrderItem>();
        }
        public long OrderId { get; set; }
        public long CustomerId { get; set; }
        public int OrderNumber { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? LastUpdated { get; set; }
        public DateTime CompleteBy { get; set; }
        public int OrderStatusId { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public int PaymentStatusId { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public double FreightPrice { get; set; }
        public MergedCustomer Customer { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        public double DiscountTotal { get { return OrderItems.Sum(i => i.Discount); } }
        public double ItemsTotal { get { return OrderItems.Sum(i => i.OriginalPrice); } }
        public double ItemsTotalAfterDiscounts { get { return OrderItems.Sum(i => i.PriceAfterDiscount); } }
    }
}
