using System;
using System.Collections.Generic;
using System.Text;

namespace Charcutarie.Models.ViewModels
{
    public class NewCorpClient
    {
        public string Name { get; set; }
        public string DBAName { get; set; }
        public bool Active { get; set; }
        public string Currency { get; set; }
    }
}
