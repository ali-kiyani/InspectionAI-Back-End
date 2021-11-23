using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using InspectionAI.AssemblyDetection.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InspectionAI.AssemblyDetection
{
    [RemoteService(false)]
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

        public async Task<PagedResultDto<DefectiveProductsResponse>> GetDefectiveProducts(PagedAssemblyDetectionResutlRequestDto request)
        {
            var data = _assemblyDetectionRepo.GetAll().Include(x => x.AssemblyDefects).ThenInclude(x => x.Defect)
                .Where(x => x.DefectsCount > 0 && x.ImageUrl != null)
                .WhereIf(request.ProductId.HasValue, x => x.ProductId == request.ProductId)
                .WhereIf(request.StageId.HasValue, x => x.StageId == request.StageId)
                .WhereIf(request.AssemblyLineId.HasValue, x => x.AssemblyLineId == request.AssemblyLineId)
                .WhereIf(request.DefectIds != null && request.DefectIds.Count > 0, x => request.DefectIds.Any(y => x.AssemblyDefects.Select(d => d.DefectId).Contains(y)))
                .OrderByDescending(x => x.DetectionTime.Date).AsQueryable();
            //data = Enumerable.Empty<AssemblyDetection>().AsQueryable();
            var totalCount = data.Count();
            
            data = ApplyPaging(data, request);

            List<DefectiveProductsResponse> defectiveList = new();
            foreach(var obj in  data)
            {
                DefectiveProductsResponse defectiveObj = new()
                {
                    AssemblyDetectionId = obj.Id,
                    DateTime = obj.DetectionTime,
                    DefectNames = obj.AssemblyDefects.Select(x => x.Defect.Name).ToList(),
                    ImageUrl = obj.ImageUrl
                };
                defectiveList.Add(defectiveObj);
            }

            var output = new PagedResultDto<DefectiveProductsResponse>(totalCount, ObjectMapper.Map<List<DefectiveProductsResponse>>(defectiveList));
            return await Task.FromResult(output);
        }
    }
}
