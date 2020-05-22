using System;

namespace Charcutarie.Models.ViewModels
{
    public class User : UpdateUser
    {
        public DateTime CreatedOn { get; set; }
        public DateTime? LastUpdated { get; set; }
        public int? CorpClientId { get; set; }
    }
}
