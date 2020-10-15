using System;
using System.Collections.Generic;
using System.Text;

namespace Charcutarie.Models.ViewModels
{
    public class Login
    {
        public string AccountNumber { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ClientSecret { get; set; }
    }
}
