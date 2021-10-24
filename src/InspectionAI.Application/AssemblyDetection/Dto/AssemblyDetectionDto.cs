using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using InspectionAI.AssemblyDefects.Dto;
using System;
using System.Collections.Generic;

namespace InspectionAI.AssemblyDetection.Dto
{
    [AutoMap(typeof(AssemblyDetection))]
    public class AssemblyDetectionDto : EntityDto<int>
    {
        public int AssemblyLineId { get; set; }
        public int ProductId { get; set; }
        public int StageId { get; set; }
        public DateTime DetectionTime { get; set; }
        public int DefectsCount { get; set; }
        public string ImageUrl { get; set; }

        public List<AssemblyDefectsDto> AssemblyDefects { get; set; }
    }
}
