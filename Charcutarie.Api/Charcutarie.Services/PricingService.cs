using Charcutarie.Application.Contracts;
using Charcutarie.Models.Entities;
using Charcutarie.Models.Enums;
using Charcutarie.Models.ViewModels;
using Charcutarie.Services.Contracts;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Charcutarie.Services
{
    public class PricingService : IPricingService
    {
        private readonly IPricingApp pricingApp;
        private readonly IMeasureUnitApp measureUnitApp;

        public PricingService(IPricingApp pricingApp, IMeasureUnitApp measureUnitApp)
        {
            this.pricingApp = pricingApp;
            this.measureUnitApp = measureUnitApp;
        }
        public double CalculatePricePerTotalWeight(PriceRequest model)
        {
            var m = measureUnitApp.GetAll().Result;
            var pType = m.FirstOrDefault(m => m.MeasureUnitId == model.ProductMeasureUnit).MeasureUnitTypeId;
            var qType = m.FirstOrDefault(m => m.MeasureUnitId == model.QuantityMeasureUnit).MeasureUnitTypeId;
            return pricingApp.CalculatePricePerTotalWeight(model,pType,qType);
        }
        public double CalculatePricePerUnit(PriceRequest model)
        {
            var m = measureUnitApp.GetAll().Result;
            var pType = m.FirstOrDefault(m => m.MeasureUnitId == model.ProductMeasureUnit).MeasureUnitTypeId;
            var qType = m.FirstOrDefault(m => m.MeasureUnitId == model.QuantityMeasureUnit).MeasureUnitTypeId;
            return pricingApp.CalculatePricePerUnit(model, pType, qType);
        }
    }
}
