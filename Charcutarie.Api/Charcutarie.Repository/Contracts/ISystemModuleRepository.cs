﻿using Charcutarie.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Charcutarie.Repository.Contracts
{
    public interface ISystemModuleRepository
    {
        Task<IEnumerable<ParentModule>> GetUserModules(long userId);
    }
}