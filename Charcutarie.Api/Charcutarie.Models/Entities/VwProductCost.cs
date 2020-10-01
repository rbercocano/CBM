using System;
using System.Collections.Generic;
using System.Text;

namespace Charcutarie.Models.Entities
{
    public class VwProductCost
    {
        public Product Product { get; set; }
        public long ProductId { get; set; }
        public decimal  Cost{ get; set; }
    }
}
