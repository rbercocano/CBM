using System;
using System.Collections.Generic;
using System.Text;

namespace Charcutarie.Models.Entities
{
    public class UserToken
    {
        public long UserId { get; set; }
        public string Token { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
