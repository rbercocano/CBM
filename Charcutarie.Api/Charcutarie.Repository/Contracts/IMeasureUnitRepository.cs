using Charcutarie.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Charcutarie.Repository.Contracts
{
    public interface IMeasureUnitRepository
    {
        public Task<IEnumerable<MeasureUnit>> GetAll();
    }
}
