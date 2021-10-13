using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using InspectionAI.AssemblyDefects.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InspectionAI.AssemblyDetection.Dto
{
    [AutoMap(typeof(AssemblyDetection))]
    public class AssemblyDetectionDto : EntityDto<int>
    {
        public int AssemblyLineId { get; set; }
        public int StageId { get; set; }
        public DateTime DetectionTime { get; set; }
        public List<AssemblyDefectsDto> AssemblyDefects { get; set; }
    }
}
