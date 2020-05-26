using Charcutarie.Models.ViewModels;
using System;
using System.Collections.Generic;

namespace Charcutarie.Models.Entities
{
    public abstract class Customer
    {
        public long CustomerId { get; set; }
        public int CustomerTypeId { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? LastUpdated { get; set; }
        public int CorpClientId { get; set; }
        public CustomerType CustomerType { get; set; }
        public List<Order> Orders { get; set; }
        public List<CustomerContact> Contacts { get; set; }

        //public string Name { get; set; }
        //public string LastName { get; set; }
        //public DateTime? DateOfBirth { get; set; }
        //public string Cpf { get; set; }

        //public string DBAName { get; set; }
        //public string Cnpj { get; set; }
    }
}
