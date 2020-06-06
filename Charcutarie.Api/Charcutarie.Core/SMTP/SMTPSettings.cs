using System;
using System.Collections.Generic;
using System.Text;

namespace Charcutarie.Core.SMTP
{
    public class SMTPSettings
    {
        public string From { get; set; }
        public string FromDisplayName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
        public string Server { get; set; }
    }
}
