using System.Threading.Tasks;
using Abp.Application.Services;
using InspectionAI.Sessions.Dto;

namespace InspectionAI.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
