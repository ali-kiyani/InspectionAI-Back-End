using Abp.Application.Services;
using Abp.Domain.Repositories;
using InspectionAI.AssemblyDetection.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InspectionAI.AssemblyDetection
{
    public class AssemblyDetectionAppService : AsyncCrudAppService<AssemblyDetection, AssemblyDetectionDto, int, PagedAssemblyDetectionResutlRequestDto, CreateAssemblyDetectionDto, AssemblyDetectionDto>, IAssemblyDetectionAppService
    {
        private readonly IRepository<AssemblyDetection> _assemblyDetectionRepo;
        private readonly IRepository<AssemblyDefects.AssemblyDefects> _assemblyDefectsRepo;

        public AssemblyDetectionAppService(IRepository<AssemblyDetection> assemblyDetectionRepo,
            IRepository<AssemblyDefects.AssemblyDefects> assemblyDefectsRepo) : base(assemblyDetectionRepo)
        {
            this._assemblyDetectionRepo = assemblyDetectionRepo;
            this._assemblyDefectsRepo = assemblyDefectsRepo;
        }

        public async Task AddNewDetectionAsync(AssemblyDetectionDto detectionDto)
        {
            await _assemblyDetectionRepo.InsertAsync(ObjectMapper.Map<AssemblyDetection>(detectionDto));
        }

        public void AddBulkDetections(List<AssemblyDetectionDto> detectionsDto)
        {
            detectionsDto.ForEach(async x =>
            {
                await _assemblyDetectionRepo.InsertAsync(ObjectMapper.Map<AssemblyDetection>(x));
            });
        }
    }
}
