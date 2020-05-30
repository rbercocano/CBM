﻿using Charcutarie.Application.Contracts;
using Charcutarie.Models.ViewModels;
using Charcutarie.Repository.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Charcutarie.Application
{
    public class DataSheetItemApp : IDataSheetItemApp
    {
        private readonly IDataSheetItemRepository repository;

        public DataSheetItemApp(IDataSheetItemRepository repository)
        {
            this.repository = repository;
        }

        public async Task<long> Add(NewDataSheetItem item, int corpClientId)
        {
            return await repository.Add(item, corpClientId);
        }
        public async Task<IEnumerable<DataSheetItem>> AddRange(IEnumerable<UpdateDataSheetItem> items, int corpClientId)
        {
            return await repository.AddRange(items, corpClientId);
        }
        public async Task<DataSheetItem> Get(long productId, int corpClientId)
        {
            return await repository.Get(productId, corpClientId);
        }
        public async Task Update(UpdateDataSheetItem item, int corpClientId)
        {
            await repository.Update(item, corpClientId);
        }
    }
}
