using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using AutoMapper;
using InspectionAI.Stage.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InspectionAI.Stage
{
    public class StageAppService : IStageAppService
    {
        private readonly IRepository<Stage> _stageRepo;

        public StageAppService(IRepository<Stage> stageRepo)
        {
            _stageRepo = stageRepo;
        }

        public async Task<ListResultDto<StageDto>> GetAllStages()
        {
            var config = new MapperConfiguration(cfg =>
                    cfg.CreateMap<Stage, StageDto>()
                );
            var mapper = new Mapper(config);
            return new ListResultDto<StageDto>(mapper.Map<List<StageDto>>(await _stageRepo.GetAllListAsync()));
        }

        public async Task<ListResultDto<StageDto>> GetProductStages(int productId)
        {
            var config = new MapperConfiguration(cfg =>
                    cfg.CreateMap<Stage, StageDto>()
                );
            var mapper = new Mapper(config);
            return new ListResultDto<StageDto>(mapper.Map<List<StageDto>>(await _stageRepo.GetAllListAsync(x => x.ProductId == productId)));
        }
    }
}
