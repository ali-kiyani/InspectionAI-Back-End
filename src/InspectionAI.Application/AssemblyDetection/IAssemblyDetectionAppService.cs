using Abp.Application.Services;
using InspectionAI.AssemblyDetection.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InspectionAI.AssemblyDetection
{
    public interface IAssemblyDetectionAppService : IAsyncCrudAppService<AssemblyDetectionDto, int, PagedAssemblyDetectionResutlRequestDto, CreateAssemblyDetectionDto, AssemblyDetectionDto>
    {
        public Task AddNewDetectionAsync(AssemblyDetectionDto detectionDto);
        public void AddBulkDetections(List<AssemblyDetectionDto> detectionsDto);
    }
}
