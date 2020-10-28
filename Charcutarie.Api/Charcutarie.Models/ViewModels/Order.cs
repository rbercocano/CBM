using Charcutarie.Models.Enums;
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
        public long OrderNumber { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? LastUpdated { get; set; }
        public DateTime CompleteBy { get; set; }
        public DateTime? PaidOn { get; set; }
        public int OrderStatusId { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public int PaymentStatusId { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public decimal FreightPrice { get; set; }
        public MergedCustomer Customer { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        public List<Transaction> Transactions { get; set; }
        public decimal Payments
        {
            get
            {
                return (Transactions ?? new List<Transaction>()).Where(t => t.IsIncome && t.TransactionStatusId == 1 && (t.IsOrderPrincipalAmount ?? false)).Sum(t => t.Amount);
            }
        }
        public decimal Tips
        {
            get
            {
                return (Transactions ?? new List<Transaction>()).Where(t => t.IsIncome && t.TransactionStatusId == 1 && !(t.IsOrderPrincipalAmount ?? false)).Sum(t => t.Amount);
            }
        }
        public decimal RemainingBalance { get { return ItemsTotalAfterDiscounts - Payments; } }

        public decimal DiscountTotal { get { return OrderItems.Where(i => i.OrderItemStatusId != OrderItemStatusEnum.Cancelado).Sum(i => i.Discount); } }
        public decimal ItemsTotal { get { return OrderItems.Where(i => i.OrderItemStatusId != OrderItemStatusEnum.Cancelado).Sum(i => i.OriginalPrice); } }
        public decimal ItemsTotalAfterDiscounts { get { return OrderItems.Where(i => i.OrderItemStatusId != OrderItemStatusEnum.Cancelado).Sum(i => i.PriceAfterDiscount); } }
        public decimal OrderTotal { get { return ItemsTotalAfterDiscounts + FreightPrice; } }

        public decimal ItemsTotalCost { get { return OrderItems.Where(i => i.Cost.HasValue && i.OrderItemStatusId != OrderItemStatusEnum.Cancelado).Sum(i => i.Cost.Value); } }
        public decimal ItemsTotalProfit { get { return OrderItems.Where(i => i.Profit.HasValue && i.OrderItemStatusId != OrderItemStatusEnum.Cancelado).Sum(i => i.Profit.Value); } }
    }
}
