using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InspectionAI.AssemblyLine.Dto
{
    [AutoMap(typeof(AssemblyLine))]
    public class AssemblyLineDto : EntityDto<int>
    {
        public string Name { get; set; }
    }
}
