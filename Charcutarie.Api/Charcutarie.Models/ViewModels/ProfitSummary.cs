﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Charcutarie.Models.ViewModels
{
    public class ProfitSummary
    {
        public Guid RowId { get; set; }
        public double TotalProfit { get; set; }
        public double CurrentMonthProfit { get; set; }
    }
}
