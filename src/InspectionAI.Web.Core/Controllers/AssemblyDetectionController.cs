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
    }
}
