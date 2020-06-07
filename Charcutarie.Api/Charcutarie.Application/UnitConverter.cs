using Charcutarie.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Charcutarie.Application
{
    public class UnitConverter
    {
        public static double ToBaseUnit(MeasureUnitEnum from, double quantity, MeasureUnitTypeEnum type)
        {
            if (type == MeasureUnitTypeEnum.Mass)
                return from switch
                {
                    MeasureUnitEnum.Grama => UnitsNet.Mass.FromGrams(quantity).Grams,
                    MeasureUnitEnum.Kilo => UnitsNet.Mass.FromKilograms(quantity).Grams,
                    MeasureUnitEnum.Libra => UnitsNet.Mass.FromPounds(quantity).Grams,
                    MeasureUnitEnum.Miligrama => UnitsNet.Mass.FromMilligrams(quantity).Grams,
                    MeasureUnitEnum.Onca => UnitsNet.Mass.FromOunces(quantity).Grams,
                    _ => 0,
                };
            if (type == MeasureUnitTypeEnum.Volume)
                return from switch
                {
                    MeasureUnitEnum.Litro => UnitsNet.Volume.FromLiters(quantity).Milliliters,
                    MeasureUnitEnum.Mililitro => UnitsNet.Volume.FromMilliliters(quantity).Milliliters,
                    _ => 0,
                };
            return 0;
        }
    }
}
