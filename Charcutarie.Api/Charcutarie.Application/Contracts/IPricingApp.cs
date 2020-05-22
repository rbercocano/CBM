using Charcutarie.Models.ViewModels;

namespace Charcutarie.Application.Contracts
{
    public interface IPricingApp
    {
        double CalculatePrice(PriceRequest model);
    }
}