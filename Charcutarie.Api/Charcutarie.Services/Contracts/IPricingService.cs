using Charcutarie.Models.ViewModels;

namespace Charcutarie.Services.Contracts
{
    public interface IPricingService
    {
        double CalculatePrice(PriceRequest model);
    }
}