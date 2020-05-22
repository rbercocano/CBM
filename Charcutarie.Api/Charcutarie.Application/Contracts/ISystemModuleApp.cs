using Charcutarie.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Charcutarie.Application.Contracts
{
    public interface ISystemModuleApp
    {
        Task<IEnumerable<ParentModule>> GetUserModules(long userId);
    }
}