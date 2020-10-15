namespace Charcutarie.Models.ViewModels
{
    public class UpdateCorpClient 
    {
        public int CorpClientId { get; set; }
        public int CustomerTypeId { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public string DBAName { get; set; }
        public string SocialIdentifier { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
    }
}
