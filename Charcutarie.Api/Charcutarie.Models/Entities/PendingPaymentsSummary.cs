using System;
using System.Collections.Generic;
using System.Text;

namespace Charcutarie.Models.Entities
{
    public class PendingPaymentsSummary
    {
        public Guid RowId { get; set; }
        public decimal TotalPendingPayments { get; set; }
        public decimal FinishedOrdersPendingPayment { get; set; }
    }
}
