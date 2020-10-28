using System;
using System.Collections.Generic;
using System.Text;

namespace Charcutarie.Models.Entities
{
    public class Transaction
    {
        public long TransactionId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public string MerchantName { get; set; }
        public int TransactionTypeId { get; set; }
        public int CorpClientId { get; set; }
        public int TransactionStatusId { get; set; }
        public long? OrderId { get; set; }
        public long UserId { get; set; }
        public User User { get; set; }
        public Order Order { get; set; }
        public TransactionType TransactionType { get; set; }
        public bool? IsOrderPrincipalAmount { get; set; }
    }
}
