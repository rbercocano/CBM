using Charcutarie.Models.ViewModels;

namespace Charcutarie.Services.Contracts
{
    public interface IPricingService
    {
        double CalculatePricePerTotalWeight(PriceRequest model);
        double CalculatePricePerUnit(PriceRequest model);
    }
}