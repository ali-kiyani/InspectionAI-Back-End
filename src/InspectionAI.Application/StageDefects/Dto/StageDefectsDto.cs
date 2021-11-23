using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using InspectionAI.Defects.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InspectionAI.StageDefects.Dto
{
    [AutoMap(typeof(StageDefects))]
    public class StageDefectsDto : EntityDto<int>
    {
        public int DefectId { get; set; }

        public DefectsDto Defects { get; set; }
    }
}
