using System;
using System.Collections.Generic;
using System.Text;

namespace Charcutarie.Models.ViewModels
{
    public class MergedCustomer
    {
        public long CustomerId { get; set; }
        public int CustomerTypeId { get; set; }
        public string Name { get; set; }
        public string CustomerType { get; set; }
        public string SocialIdentifier { get; set; }
        public List<CustomerContact> Contacts { get; set; }
    }
}
