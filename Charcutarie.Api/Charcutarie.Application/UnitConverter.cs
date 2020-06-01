using Charcutarie.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Charcutarie.Application
{
    public class UnitConverter
    {
        public static double ToBaseUnit(MeasureUnitEnum from, double quantity)
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
