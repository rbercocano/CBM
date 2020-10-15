using System;
using System.Text.Json.Serialization;

namespace Charcutarie.Models.ViewModels
{
    public class NewUser
    {
        public string Name { get; set; }
        public string Username { get; set; }
        public bool Active { get; set; }
        public int RoleId { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string HomePhone { get; set; }
        public string Mobile { get; set; }
        public int DefaultMassMid { get; set; }
        public int DefaultVolumeMid { get; set; }
    }
}
