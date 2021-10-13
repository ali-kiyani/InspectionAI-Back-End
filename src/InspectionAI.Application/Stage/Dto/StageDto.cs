using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InspectionAI.Stage.Dto
{
    [AutoMap(typeof(Stage))]
    public class StageDto : EntityDto<int>
    {
        public string Name { get; set; }
    }
}
