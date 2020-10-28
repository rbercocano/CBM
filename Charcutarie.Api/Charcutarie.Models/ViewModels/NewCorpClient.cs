using System;
using System.Collections.Generic;
using System.Text;

namespace Charcutarie.Models.ViewModels
{
    public class NewCorpClient
    {
        public string Name { get; set; }
        public string DBAName { get; set; }
        public string Email { get; set; }
        public bool Active { get; set; }
        public string Currency { get; set; }
        public List<Customer> Customers { get; set; }
        public int CustomerTypeId { get; set; }
        public string CPF { get; set; }
        public string CNPJ { get; set; }
        public string Mobile { get; set; }
        public DateTimeOffset? LicenseExpirationDate { get; set; }
    }
}
