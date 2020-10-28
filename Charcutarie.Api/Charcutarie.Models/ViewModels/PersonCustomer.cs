using System;
using System.Collections.Generic;
using System.Text;

namespace Charcutarie.Models.ViewModels
{
    public class PersonCustomer : UpdatePersonCustomer
    {
        public DateTime CreatedOn {  get; set; }
        public DateTime? LastUpdated { get; set; }
        public new int CorpClientId { get; set; }
        public string CustomerType { get; set; }
    }
}
