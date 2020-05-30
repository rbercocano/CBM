using Charcutarie.Application.Contracts;
using Charcutarie.Models.ViewModels;
using Charcutarie.Repository.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Charcutarie.Application
{
    public class DataSheetApp : IDataSheetApp
    {
        private readonly IDataSheetRepository repository;

        public DataSheetApp(IDataSheetRepository repository)
        {
            this.repository = repository;
        }

        public async Task<DataSheet> Get(long productId, int corpClientId)
        {
            return await repository.Get(productId, corpClientId);
        }
        public async Task<DataSheet> Save(SaveDataSheet dataSheet, int corpClientId)
        {
            return await repository.Save(dataSheet, corpClientId);
        }
    }
}
