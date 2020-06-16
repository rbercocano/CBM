using Charcutarie.Application;
using Charcutarie.Application.Contracts;
using Charcutarie.Models.Enums;
using Charcutarie.Models.ViewModels;
using Charcutarie.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Charcutarie.Services
{
    public class DataSheetService : IDataSheetService
    {
        private readonly IDataSheetApp dataSheetApp;
        private readonly IDataSheetItemApp dataSheetItemApp;
        private readonly IPricingApp pricingApp;
        private readonly IMeasureUnitApp measureUnitApp;
        private readonly IProductApp productApp;

        public DataSheetService(IDataSheetApp dataSheetApp, IDataSheetItemApp dataSheetItemApp, IPricingApp pricingApp, IMeasureUnitApp measureUnitApp, IProductApp productApp)
        {
            this.dataSheetApp = dataSheetApp;
            this.dataSheetItemApp = dataSheetItemApp;
            this.pricingApp = pricingApp;
            this.measureUnitApp = measureUnitApp;
            this.productApp = productApp;
        }
        public async Task<long> Create(DataSheet dataSheet, int corpClientId)
        {
            var result = await dataSheetApp.Save(new SaveDataSheet { ProcedureDescription = dataSheet.ProcedureDescription, ProductId = dataSheet.ProductId }, corpClientId);
            var items = dataSheet.DataSheetItems.Select(i => new NewDataSheetItem
            {
                ProductId = dataSheet.ProductId,
                AdditionalInfo = i.AdditionalInfo,
                Percentage = i.Percentage,
                RawMaterialId = i.RawMaterialId
            }).ToList();
            if (items.Any())
                await dataSheetItemApp.AddRange(items, corpClientId);
            return result.DataSheetId;
        }
        public async Task<long> Update(SaveDataSheet saveDataSheet, int corpClientId)
        {
            var result = await dataSheetApp.Save(saveDataSheet, corpClientId);
            return result.DataSheetId;
        }
        public async Task<long> AddItem(NewDataSheetItem item, int corpClientId)
        {
            var result = await dataSheetItemApp.Add(item, corpClientId);
            return result;
        }
        public async Task UpdateItem(UpdateDataSheetItem item, int corpClientId)
        {
            await dataSheetItemApp.Update(item, corpClientId);
        }
        public async Task DeleteItem(long itemId, int corpClientId)
        {
            await dataSheetItemApp.Delete(itemId, corpClientId);
        }
        public async Task<DataSheetItem> GetDataSheetItem(long itemId, int corpClientId)
        {
            return await dataSheetItemApp.Get(itemId, corpClientId);
        }
        public async Task<DataSheet> GetDataSheet(long productId, int corpClientId)
        {
            return await dataSheetApp.Get(productId, corpClientId);
        }
        public async Task<ProductionSummary> CalculateProduction(long productId, MeasureUnitEnum measureId, double quantity, int corpClientId)
        {
            var items = new List<ProductionItem>();
            var dataSheet = await dataSheetApp.Get(productId, corpClientId);
            var units = await measureUnitApp.GetAll();
            var sourceUnit = units.FirstOrDefault(u => u.MeasureUnitId == measureId);
            var p = await productApp.Get(corpClientId, productId);
            var pType = units.FirstOrDefault(u => u.MeasureUnitId == p.MeasureUnitId).MeasureUnitTypeId;
            var price = pricingApp.CalculatePricePerTotalWeight(new PriceRequest
            {
                ProductMeasureUnit = p.MeasureUnitId,
                ProductPrice = p.Price,
                Quantity = quantity,
                QuantityMeasureUnit = measureId,
                ResultPrecision = 2
            }, pType, sourceUnit.MeasureUnitTypeId);
            if (dataSheet == null)
                return new ProductionSummary
                {
                    SalePrice = price,
                    ProductionItems = items
                };
            var q = UnitConverter.ToBaseUnit(measureId, quantity, sourceUnit.MeasureUnitTypeId);
            foreach (var di in dataSheet.DataSheetItems)
            {
                var item = new ProductionItem
                {
                    AdditionalInfo = di.AdditionalInfo,
                    DataSheetId = di.DataSheetId,
                    DataSheetItemId = di.DataSheetItemId,
                    IsBaseItem = di.IsBaseItem,
                    RawMaterial = di.RawMaterial,
                    Percentage = di.Percentage,
                    RawMaterialId = di.RawMaterialId
                };
                if (dataSheet.IncreaseWeight)
                    item.Quantity = q / (1 + (dataSheet.WeightVariationPercentage / 100)) * item.Percentage / 100;
                else
                    item.Quantity = q / (1 - (dataSheet.WeightVariationPercentage / 100)) * item.Percentage / 100;

                var rmSourceUnit = units.FirstOrDefault(u => u.MeasureUnitId == di.RawMaterial.MeasureUnitId);
                var rmQuantity = UnitConverter.ToBaseUnit(rmSourceUnit.MeasureUnitId, 1, rmSourceUnit.MeasureUnitTypeId);
                var rmPrice = di.RawMaterial.Price / rmQuantity;
                item.Cost = rmPrice * item.Quantity;

                switch (rmSourceUnit.MeasureUnitTypeId)
                {
                    case MeasureUnitTypeEnum.Mass:
                        item.MeasureUnit = units.FirstOrDefault(u => u.MeasureUnitId == MeasureUnitEnum.Grama);
                        break;
                    case MeasureUnitTypeEnum.Volume:
                        item.MeasureUnit = units.FirstOrDefault(u => u.MeasureUnitId == MeasureUnitEnum.Mililitro);
                        break;
                    case MeasureUnitTypeEnum.Lenght:
                        break;
                    default:
                        break;
                }
                items.Add(item);
            }
            return new ProductionSummary
            {
                SalePrice = price,
                ProductionItems = items
            };
        }
    }
}
