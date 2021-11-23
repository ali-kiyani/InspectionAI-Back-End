using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InspectionAI.AssemblyDefects.Dto
{
    public class PagedAssemblyDefectsResultRequestDto : PagedResultRequestDto
    {
        public int ProductId { get; set; }
        public int StageId { get; set; }
        public int AssemblyLineId { get; set; }
        public int DefectId { get; set; }
    }
}
