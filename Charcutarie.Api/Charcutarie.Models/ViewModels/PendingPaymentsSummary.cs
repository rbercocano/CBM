using System;
using System.Collections.Generic;
using System.Text;

namespace Charcutarie.Models.ViewModels
{
    public class PendingPaymentsSummary
    {
        public Guid RowId { get; set; }
        public double TotalPendingPayments { get; set; }
        public double FinishedOrdersPendingPayment { get; set; }
    }
}
