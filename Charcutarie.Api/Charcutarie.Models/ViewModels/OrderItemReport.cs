using System;
using System.Collections.Generic;
using System.Text;

namespace Charcutarie.Models.ViewModels
{
    public class OrderItemReport
    {
        public long OrderId { get; set; }
        public int OrderNumber { get; set; }
        public int CustomerTypeId { get; set; }
        public string OrderStatus { get; set; }
        public string Customer { get; set; }
        public string SocialIdentifier { get; set; }
        public int ItemNumber { get; set; }
        public string Product { get; set; }
        public double Quantity { get; set; }
        public string MeasureUnit { get; set; }
        public string OrderItemStatus { get; set; }
        public int OrderItemStatusId { get; set; }
        public double FinalPrice { get; set; }
        public DateTime LastStatusDate { get; set; }
        public DateTime CompleteBy { get; set; }
    }
}
