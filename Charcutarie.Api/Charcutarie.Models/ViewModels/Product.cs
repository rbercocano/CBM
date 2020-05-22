namespace Charcutarie.Models.ViewModels
{
    public class Product : UpdateProduct
    {
        public string MeasureUnit { get; set; }
        public new int CorpClientId { get; set; }
    }
}
