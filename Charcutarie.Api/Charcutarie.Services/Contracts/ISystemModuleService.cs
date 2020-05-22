using Charcutarie.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Charcutarie.Services.Contracts
{
    public interface ISystemModuleService
    {
        Task<IEnumerable<ParentModule>> GetUserModules(long userId);
    }
}