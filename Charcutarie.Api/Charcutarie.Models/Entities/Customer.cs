using Charcutarie.Models.ViewModels;
using System;
using System.Collections.Generic;

namespace Charcutarie.Models.Entities
{
    public abstract class Customer
    {
        public long CustomerId { get; set; }
        public int CustomerTypeId { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset? LastUpdated { get; set; }
        public int CorpClientId { get; set; }
        public CustomerType CustomerType { get; set; }
        public List<Order> Orders { get; set; }
        public List<CustomerContact> Contacts { get; set; }
    }
}
