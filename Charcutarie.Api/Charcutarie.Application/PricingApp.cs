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
        public double CalculatePricePerTotalWeight(PriceRequest model)
        {

            var weightInGrams = ToBaseUnit(model.ProductMeasureUnit, 1);
            var pricePerGram = model.ProductPrice / weightInGrams;
            var orderedWeight = ToBaseUnit(model.QuantityMeasureUnit, model.Quantity);
            var totalPrice = orderedWeight * pricePerGram;
            return Math.Round(totalPrice, model.ResultPrecision);
        }
        public double CalculatePricePerUnit(PriceRequest model)
        {

            var weightInGrams = ToBaseUnit(model.ProductMeasureUnit, model.Quantity);
            var pricePerGram = model.ProductPrice / weightInGrams;
            var orderedWeight = ToBaseUnit(model.QuantityMeasureUnit, 1);
            var totalPrice = orderedWeight * pricePerGram;
            return Math.Round(totalPrice, model.ResultPrecision);
        }
        private double ToBaseUnit(MeasureUnitEnum from, double quantity)
        {
            return from switch
            {
                MeasureUnitEnum.Grama => UnitsNet.Mass.FromGrams(quantity).Grams,
                MeasureUnitEnum.Kilo => UnitsNet.Mass.FromKilograms(quantity).Grams,
                MeasureUnitEnum.Libra => UnitsNet.Mass.FromPounds(quantity).Grams,
                MeasureUnitEnum.Miligrama => UnitsNet.Mass.FromMilligrams(quantity).Grams,
                MeasureUnitEnum.Litro => UnitsNet.Volume.FromLiters(quantity).Milliliters,
                MeasureUnitEnum.Mililitro => UnitsNet.Volume.FromLiters(quantity).Milliliters,
                MeasureUnitEnum.Onca => UnitsNet.Mass.FromOunces(quantity).Grams,
                _ => 0,
            };
        }
    }
}
