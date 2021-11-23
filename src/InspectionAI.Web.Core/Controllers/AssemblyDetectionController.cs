using Abp.Application.Services.Dto;
using InspectionAI.AssemblyDetection;
using InspectionAI.AssemblyDetection.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InspectionAI.Controllers
{
    [Route("api/[controller]/[action]")]
    public class AssemblyDetectionController : InspectionAIControllerBase
    {
        private readonly IAssemblyDetectionAppService _appService;

        public AssemblyDetectionController(IAssemblyDetectionAppService appService)
        {
            this._appService = appService;
        }

        [HttpPost]
        public async Task<ActionResult> AddAssemblyDetection([FromBody] AssemblyDetectionDto assemblyDetectionDto)
        {
            await _appService.AddNewDetectionAsync(assemblyDetectionDto);
            return Ok();
        }

        [HttpPost]
        public ActionResult AddBulkDetections([FromBody] List<AssemblyDetectionDto> assemblyDetectionsDto)
        {
            _appService.AddBulkDetections(assemblyDetectionsDto);
            return Ok();
        }

        [HttpPost]
        public async Task<PagedResultDto<DefectiveProductsResponse>> GetDefectiveProducts([FromBody] PagedAssemblyDetectionResutlRequestDto request)
        {
            return await _appService.GetDefectiveProducts(request);
        }
    }
}
