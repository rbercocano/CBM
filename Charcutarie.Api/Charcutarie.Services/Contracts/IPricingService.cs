using Charcutarie.Models.ViewModels;

namespace Charcutarie.Services.Contracts
{
    public interface IPricingService
    {
        decimal CalculatePricePerTotalWeight(PriceRequest model);
        decimal CalculatePricePerUnit(PriceRequest model);
    }
}