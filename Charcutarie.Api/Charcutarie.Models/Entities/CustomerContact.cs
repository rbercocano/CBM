using System;
using System.Collections.Generic;
using System.Text;

namespace Charcutarie.Models.Entities
{
    public class CustomerContact
    {
        public long CustomerContactId { get; set; }
        public long CustomerId { get; set; }
        public int ContactTypeId { get; set; }
        public string Contact { get; set; }
        public string AdditionalInfo { get; set; }
        public ContactType ContactType { get; set; }
        public Customer Customer { get; set; }
    }
}
