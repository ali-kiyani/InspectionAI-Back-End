using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InspectionAI.AssemblyDetection.Dto
{
    public class PagedAssemblyDetectionResutlRequestDto : PagedResultRequestDto
    {
        public int? ProductId { get; set; }
        public int? StageId { get; set; }
        public int? AssemblyLineId { get; set; }
        public List<int> DefectIds { get; set; }
    }
}
