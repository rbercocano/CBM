﻿using Charcutarie.Models.Enums;
using System;

namespace Charcutarie.Models.Entities
{
    public class Product
    {
        public long ProductId { get; set; }
        public string Name { get; set; }
        public MeasureUnitEnum MeasureUnitId { get; set; }
        public decimal Price { get; set; }
        public bool ActiveForSale { get; set; }
        public int CorpClientId { get; set; }

        public MeasureUnit MeasureUnit { get; set; }
        public DataSheet DataSheet { get; set; }
        public VwProductCost ProductCost { get; set; }
    }
}
