using Charcutarie.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Charcutarie.Models.ViewModels
{
    public class NewOrder
    {
        public NewOrder()
        {
            OrderItems = new List<NewOrderItem>();
        }
        public long CustomerId { get; set; }
        public DateTime CompleteBy { get; set; }
        public PaymentStatusEnum PaymentStatusId { get; set; }
        public decimal FreightPrice { get; set; }
        public List<NewOrderItem> OrderItems { get; set; }

        [JsonIgnore]
        public int OrderStatusId { get; set; }
        [JsonIgnore]
        public int OrderNumber { get; set; }
    }
}
