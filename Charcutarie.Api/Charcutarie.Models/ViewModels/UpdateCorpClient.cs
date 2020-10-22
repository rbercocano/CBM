namespace Charcutarie.Models.ViewModels
{
    public class UpdateCorpClient 
    {
        public int CorpClientId { get; set; }
        public int CustomerTypeId { get; set; }
        public string Name { get; set; }
        public string DbaName { get; set; }
        public string SocialIdentifier { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Currency { get; set; }
    }
}
