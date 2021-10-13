using System.Threading.Tasks;
using Abp.Application.Services;
using InspectionAI.Authorization.Accounts.Dto;

namespace InspectionAI.Authorization.Accounts
{
    public interface IAccountAppService : IApplicationService
    {
        Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

        Task<RegisterOutput> Register(RegisterInput input);
    }
}
