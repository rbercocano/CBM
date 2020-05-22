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
        public double Quantity { get; set; }
        public string AdditionalInfo { get; set; }
        public double Discount { get; set; }
        public double OriginalPrice { get; set; }
        public double PriceAfterDiscount { get; set; }
        public double ProductPrice { get; set; }
        public int ItemNumber { get; set; }
    }
}
