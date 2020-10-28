using System;
using System.Collections.Generic;

namespace Charcutarie.Models.Entities
{
    public class CorpClient
    {
        public int CorpClientId { get; set; }
        public string Name { get; set; }
        public string DBAName { get; set; }
        public bool Active { get; set; }
        public string Currency { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset? LastUpdated { get; set; }
        public List<Customer> Customers { get; set; }
        public int CustomerTypeId { get; set; }
        public string CPF { get; set; }
        public string CNPJ { get; set; }
        public DateTimeOffset? LicenseExpirationDate { get; set; }
        public string AccountNumber { get; private set; }
    }
}
