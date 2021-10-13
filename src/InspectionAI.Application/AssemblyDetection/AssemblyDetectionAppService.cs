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

        public async Task AddNewDetectionAsync(CreateAssemblyDetectionDto detectionDto)
        {
            Task<int> waitId = _assemblyDetectionRepo.InsertAndGetIdAsync(ObjectMapper.Map<AssemblyDetection>(detectionDto));
            List<AssemblyDefects.AssemblyDefects> defects = ObjectMapper.Map<List<AssemblyDefects.AssemblyDefects>>(detectionDto.AssemblyDefects);
            int id = await waitId;
            defects.ForEach(async x => {
                x.AssemblyDetectionId = id;
                await _assemblyDefectsRepo.InsertAsync(ObjectMapper.Map<AssemblyDefects.AssemblyDefects>(x));
                });
        }
    }
}
