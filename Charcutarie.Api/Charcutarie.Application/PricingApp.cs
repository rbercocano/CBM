using Charcutarie.Application.Contracts;
using Charcutarie.Models.Enums;
using Charcutarie.Models.ViewModels;
using Charcutarie.Repository.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Charcutarie.Application
{
    public class PricingApp : IPricingApp
    {
        public decimal CalculatePricePerTotalWeight(PriceRequest model, MeasureUnitTypeEnum pType, MeasureUnitTypeEnum qType)
        {
            if (model.ResultPrecision == 0)
                model.ResultPrecision = 2;
            var weightInGrams = Convert.ToDecimal(UnitConverter.ToBaseUnit(model.ProductMeasureUnit, 1, pType));
            var pricePerUnit = model.ProductPrice / weightInGrams;
            var orderedWeight = Convert.ToDecimal(UnitConverter.ToBaseUnit(model.QuantityMeasureUnit, model.Quantity, qType));
            var totalPrice = orderedWeight * pricePerUnit;
            return Math.Round(totalPrice, model.ResultPrecision);
        }
        public decimal CalculatePricePerUnit(PriceRequest model, MeasureUnitTypeEnum pType, MeasureUnitTypeEnum qType)
        {
            if (model.ResultPrecision == 0)
                model.ResultPrecision = 2;
            var weightInGrams = Convert.ToDecimal(UnitConverter.ToBaseUnit(model.ProductMeasureUnit, model.Quantity, pType));
            var pricePerUnit = model.ProductPrice / weightInGrams;
            var orderedWeight = Convert.ToDecimal(UnitConverter.ToBaseUnit(model.QuantityMeasureUnit, 1, qType));
            var totalPrice = orderedWeight * pricePerUnit;
            return Math.Round(totalPrice, model.ResultPrecision);
        }
    }
}
