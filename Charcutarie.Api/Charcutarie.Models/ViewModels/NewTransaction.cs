using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Charcutarie.Models.ViewModels
{
    public class NewTransaction
    {
        public decimal Amount { get; set; }
        public DateTimeOffset Date { get; set; }
        public string Description { get; set; }
        public string MerchantName { get; set; }
        public int TransactionTypeId { get; set; }       
        public long? OrderId { get; set; }

        [JsonIgnore]
        public bool? IsOrderPrincipalAmount { get; set; }
        [JsonIgnore]
        public int TransactionStatusId { get; set; }
        [JsonIgnore]
        public int CorpClientId { get; set; }
        [JsonIgnore]
        public long UserId { get; set; }
    }
}
