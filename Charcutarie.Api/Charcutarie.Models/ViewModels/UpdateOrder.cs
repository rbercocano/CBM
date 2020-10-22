using Charcutarie.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Charcutarie.Models.ViewModels
{
    public class UpdateOrder
    {
        public DateTime CompleteBy { get; set; }
        public PaymentStatusEnum PaymentStatusId { get; set; }
        public decimal FreightPrice { get; set; }
        public long OrderNumber { get; set; }
    }
}
