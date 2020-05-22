﻿
using Charcutarie.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Charcutarie.Application.Contracts
{
    public interface IMeasureUnitApp
    {
        Task<IEnumerable<MeasureUnit>> GetAll();
    }
}
