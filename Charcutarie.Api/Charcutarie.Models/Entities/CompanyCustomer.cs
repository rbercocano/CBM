using System;
using System.Collections.Generic;
using System.Text;

namespace Charcutarie.Models.Entities
{
    public class CompanyCustomer : Customer
    {
        public string DBAName { get; set; }
        public string Name { get; set; }
        public string Cnpj { get; set; }
    }
}
