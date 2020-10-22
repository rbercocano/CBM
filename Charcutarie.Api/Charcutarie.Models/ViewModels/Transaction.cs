using System;
using System.Collections.Generic;
using System.Text;

namespace Charcutarie.Models.ViewModels
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
        public long? OrderId { get; set; }
        public int TransactionStatusId { get; set; }
        public string TransactionType { get; set; }
        public bool IsIncome { get; set; }
        public long UserId { get; set; }
        public string Username { get; set; }
        public string UserFullName { get; set; }
        public bool? IsOrderPrincipalAmount { get; set; }
        
    }
}
