using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Charcutarie.Models.ViewModels
{
    public class ClientRegistration
    {
        public int CustomerTypeId { get; set; }
        public string Name { get; set; }
        public string DbaName { get; set; }
        public string SocialIdentifier { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        [JsonIgnore]
        public string Username { get; set; }
        public string Password { get; set; }
        public string PasswordConfirmation { get; set; }
    }
}
