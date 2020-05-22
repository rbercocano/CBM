using System;
using System.Collections.Generic;
using System.Text;

namespace Charcutarie.Models.ViewModels
{
    public class CorpClient : NewCorpClient
    {
        public int CorpClientId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
