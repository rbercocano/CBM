using System;
using System.Collections.Generic;
using System.Text;

namespace Charcutarie.Models.ViewModels
{
    public class PayOrder
    {
        public long OrderNumber { get; set; }
        public decimal Amount { get; set; }
        public decimal Tip { get; set; }
        public int OrderPaymentMethod { get; set; }
        public int TipPaymentMethod { get; set; }
    }
}
