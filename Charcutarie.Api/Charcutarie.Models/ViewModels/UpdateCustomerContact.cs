using System;
using System.Collections.Generic;
using System.Text;

namespace Charcutarie.Models.ViewModels
{
    public class UpdateCustomerContact: AddCustomerContact
    {
        public long CustomerContactId { get; set; }
    }
}
