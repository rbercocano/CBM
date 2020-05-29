using Charcutarie.Models.ViewModels;

namespace Charcutarie.Application.Contracts
{
    public interface IPricingApp
    {
        double CalculatePricePerTotalWeight(PriceRequest model);
        double CalculatePricePerUnit(PriceRequest model);
    }
}