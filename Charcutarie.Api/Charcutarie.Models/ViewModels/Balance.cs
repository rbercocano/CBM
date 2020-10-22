using System;
using System.Collections.Generic;
using System.Text;

namespace Charcutarie.Models.ViewModels
{
    public class Balance
    {
        public long RID { get; set; }
        public long? TransactionId { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal Amount { get; set; }
        public decimal RemainingBalance { get; set; }
        public string Description { get; set; }
        public string MerchantName { get; set; }
        public int? TransactionTypeId { get; set; }
        public long? OrderId { get; set; }
        public string TransactionType { get; set; }
        public long UserId { get; set; }
        public string Username { get; set; }
        public string UserFullName { get; set; }
    }
}
