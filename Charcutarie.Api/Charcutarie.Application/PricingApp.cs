﻿using Charcutarie.Application.Contracts;
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

            var weightInGrams = UnitConverter.ToBaseUnit(model.ProductMeasureUnit, 1, MeasureUnitTypeEnum.Mass);
            var pricePerGram = model.ProductPrice / weightInGrams;
            var orderedWeight = UnitConverter.ToBaseUnit(model.QuantityMeasureUnit, model.Quantity, MeasureUnitTypeEnum.Mass);
            var totalPrice = orderedWeight * pricePerGram;
            return Math.Round(totalPrice, model.ResultPrecision);
        }
        public double CalculatePricePerUnit(PriceRequest model)
        {

            var weightInGrams = UnitConverter.ToBaseUnit(model.ProductMeasureUnit, model.Quantity, MeasureUnitTypeEnum.Mass);
            var pricePerGram = model.ProductPrice / weightInGrams;
            var orderedWeight = UnitConverter.ToBaseUnit(model.QuantityMeasureUnit, 1, MeasureUnitTypeEnum.Mass);
            var totalPrice = orderedWeight * pricePerGram;
            return Math.Round(totalPrice, model.ResultPrecision);
        }
    }
}
