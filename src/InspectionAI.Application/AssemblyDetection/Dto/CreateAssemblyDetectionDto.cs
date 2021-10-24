using Abp.AutoMapper;
using InspectionAI.AssemblyDefects.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InspectionAI.AssemblyDetection.Dto
{

    public class CreateAssemblyDetectionDto
    {
        public int AssemblyLineId { get; set; }
        public int ProductId { get; set; }
        public int StageId { get; set; }
        public DateTime DetectionTime { get; set; }
        public int DefectsCount { get; set; }
        public List<AssemblyDefectsDto> AssemblyDefects { get; set; }
    }
}
