﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Charcutarie.Models.Entities
{
    public class SalesPerMonth
    {
        public DateTime Date { get; set; }
        public double TotalSales { get; set; }
        public double TotalProfit { get; set; }
    }
}