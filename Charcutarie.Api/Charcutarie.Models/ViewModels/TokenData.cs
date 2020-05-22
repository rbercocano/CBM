using System;
using System.Collections.Generic;
using System.Text;

namespace Charcutarie.Models.ViewModels
{
    public class TokenData
    {
        public JWTUserInfo UserData { get; set; }
        public bool Authenticated { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Expiration { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string Message { get; set; }
    }
}
