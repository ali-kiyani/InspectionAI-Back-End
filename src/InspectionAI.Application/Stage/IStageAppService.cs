using Abp.Application.Services;
using Abp.Application.Services.Dto;
using InspectionAI.Stage.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InspectionAI.Stage
{
    public interface IStageAppService : IApplicationService
    {
        Task<ListResultDto<StageDto>> GetAllStages();
        Task<ListResultDto<StageDto>> GetProductStages(int productId);
    }
}
