using System;
using System.Collections.Generic;
using System.Text;

namespace Charcutarie.Models.ViewModels
{
    public class RequestTokenRefresh
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
