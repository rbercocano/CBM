using System;
using System.Collections.Generic;
using System.Text;

namespace Charcutarie.Models
{
    public class UserInfo
    {
        public int? CorpClientId { get; set; }
        public int RoleId { get; set; }
        public long UserId { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
    }
}
