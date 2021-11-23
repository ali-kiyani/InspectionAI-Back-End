using Abp.Application.Services;
using InspectionAI.AssemblyDefects.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InspectionAI.AssemblyDefects
{
    public interface IAssemblyDefectsAppService : IAsyncCrudAppService<AssemblyDefectsDto, int, PagedAssemblyDefectsResultRequestDto, CreateAssemblyDefectsDto, AssemblyDefectsDto>
    {
    }
}
