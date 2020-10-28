using System;
using System.Collections.Generic;
using System.Text;

namespace Charcutarie.Models.ViewModels
{
    public class CorpClient 
    {
        public int CorpClientId { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string DbaName { get; set; }
        public string Mobile { get; set; }
        public bool Active { get; set; }
        public string Currency { get; set; }
        public int CustomerTypeId { get; set; }
        public string CPF { get; set; }
        public string CNPJ { get; set; }
        public DateTimeOffset? LicenseExpirationDate { get; set; }
        public DateTimeOffset? LastUpdated { get; set; }
        public string AccountNumber { get; set; }
    }
}
