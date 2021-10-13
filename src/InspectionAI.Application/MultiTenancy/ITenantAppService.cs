using Abp.Application.Services;
using InspectionAI.MultiTenancy.Dto;

namespace InspectionAI.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}

