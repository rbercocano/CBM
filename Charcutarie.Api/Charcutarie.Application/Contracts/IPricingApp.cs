using Charcutarie.Models.Enums;
using Charcutarie.Models.ViewModels;

namespace Charcutarie.Application.Contracts
{
    public interface IPricingApp
    {
        decimal CalculatePricePerTotalWeight(PriceRequest model, MeasureUnitTypeEnum pType, MeasureUnitTypeEnum qType);
        decimal CalculatePricePerUnit(PriceRequest model, MeasureUnitTypeEnum pType, MeasureUnitTypeEnum qType);
    }
}