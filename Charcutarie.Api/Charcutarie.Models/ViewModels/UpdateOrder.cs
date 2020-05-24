using System;
using System.Collections.Generic;
using System.Text;

namespace Charcutarie.Models.ViewModels
{
    public class UpdateOrder
    {
        public DateTime CompleteBy { get; set; }
        public int PaymentStatusId { get; set; }
        public decimal FreightPrice { get; set; }
        public int OrderNumber { get; set; }
    }
}
