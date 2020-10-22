using System;
using System.Collections.Generic;
using System.Text;

namespace Charcutarie.Models.ViewModels
{
    public class RefundPayment
    {
        public long OrderNumber { get; set; }
        public long TransactionId { get; set; }
    }
}
