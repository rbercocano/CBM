using System;
using System.Collections.Generic;
using System.Text;

namespace Charcutarie.Models.ViewModels
{
    public class CustomerContact
    {
        public long CustomerContactId { get; set; }
        public long CustomerId { get; set; }
        public int ContactTypeId { get; set; }
        public string Contact { get; set; }
        public string AdditionalInfo { get; set; }
        public string ContactType { get; set; }
        public string ContactIcon { get; set; }
    }
}
