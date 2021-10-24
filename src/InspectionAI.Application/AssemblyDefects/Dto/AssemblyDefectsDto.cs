using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InspectionAI.AssemblyDefects.Dto
{
    [AutoMap(typeof(AssemblyDefects))]
    public class AssemblyDefectsDto : EntityDto<int>
    {
        public int AssemblyDetectionId { get; set; }
        public int StageId { get; set; }
        public int DefectId { get; set; }
        public float Confidence { get; set; }
        public DateTime DetectionTime { get; set; }

    }
}
