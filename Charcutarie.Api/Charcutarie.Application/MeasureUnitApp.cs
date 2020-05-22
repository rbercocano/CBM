using Charcutarie.Application.Contracts;
using Charcutarie.Models.ViewModels;
using Charcutarie.Repository.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Charcutarie.Application
{
    public class MeasureUnitApp : IMeasureUnitApp
    {
        private readonly IMeasureUnitRepository repository;

        public MeasureUnitApp(IMeasureUnitRepository repository)
        {
            this.repository = repository;
        }
        public async Task<IEnumerable<MeasureUnit>> GetAll()
        {
            return await repository.GetAll();
        }
    }
}
