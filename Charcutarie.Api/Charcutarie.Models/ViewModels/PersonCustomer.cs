using System;
using System.Collections.Generic;
using System.Text;

namespace Charcutarie.Models.ViewModels
{
    public class PersonCustomer : UpdatePersonCustomer
    {
        public DateTimeOffset CreatedOn {  get; set; }
        public DateTimeOffset? LastUpdated { get; set; }
        public new int CorpClientId { get; set; }
        public string CustomerType { get; set; }
    }
}
