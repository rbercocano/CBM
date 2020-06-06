using Charcutarie.Models.Enums;
using Charcutarie.Models.ViewModels;

namespace Charcutarie.Application.Contracts
{
    public interface IPricingApp
    {
        double CalculatePricePerTotalWeight(PriceRequest model, MeasureUnitTypeEnum pType, MeasureUnitTypeEnum qType);
        double CalculatePricePerUnit(PriceRequest model, MeasureUnitTypeEnum pType, MeasureUnitTypeEnum qType);
    }
}