using Charcutarie.Application.Contracts;
using Charcutarie.Models.Entities;
using Charcutarie.Models.Enums;
using Charcutarie.Models.ViewModels;
using Charcutarie.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Charcutarie.Services
{
    public class PricingService : IPricingService
    {
        private readonly IPricingApp pricingApp;

        public PricingService(IPricingApp pricingApp)
        {
            this.pricingApp = pricingApp;
        }
        public double CalculatePrice(PriceRequest model)
        {
            return pricingApp.CalculatePrice(model);
        }
    }
}
