using System;
using System.Collections.Generic;
using System.Text;

namespace Charcutarie.Models.ViewModels
{
    public class NewCompanyCustomer : Customer
    {
        public string Name { get; set; }
        public string DBAName { get; set; }
        public string Cnpj { get; set; }
    }
}
