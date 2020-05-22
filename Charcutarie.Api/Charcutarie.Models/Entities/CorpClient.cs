using System;
using System.Collections.Generic;
using System.Text;

namespace Charcutarie.Models.Entities
{
    public class CorpClient
    {
        public int CorpClientId { get; set; }
        public string Name { get; set; }
        public string DBAName { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
