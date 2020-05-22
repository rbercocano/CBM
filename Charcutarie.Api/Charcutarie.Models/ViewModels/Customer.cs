using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Charcutarie.Models.ViewModels
{
    public  class Customer
    {
        [JsonIgnore]
        public int CustomerTypeId { get; set; }
        [JsonIgnore]
        public int CorpClientId { get; set; }
    }
}
