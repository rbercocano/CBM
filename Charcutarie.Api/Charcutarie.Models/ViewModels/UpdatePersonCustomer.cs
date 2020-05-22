using System;
using System.Collections.Generic;
using System.Text;

namespace Charcutarie.Models.ViewModels
{
    public class UpdatePersonCustomer : NewPersonCustomer
    {
        public long CustomerId { get; set; }
    }
}
