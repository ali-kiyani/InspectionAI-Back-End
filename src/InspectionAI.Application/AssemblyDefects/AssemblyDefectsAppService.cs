using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.EntityFrameworkCore;
using Abp.EntityFrameworkCore.Uow;
using InspectionAI.AssemblyDefects.Dto;
using InspectionAI.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InspectionAI.AssemblyDefects
{
    [RemoteService(false)]
    public class AssemblyDefectsAppService : AsyncCrudAppService<AssemblyDefects, AssemblyDefectsDto, int, PagedAssemblyDefectsResultRequestDto, CreateAssemblyDefectsDto, AssemblyDefectsDto>, IAssemblyDefectsAppService
    {
        private readonly IRepository<AssemblyDefects> _assemblyDefectsRepo;
        private readonly IRepository<AssemblyDetection.AssemblyDetection> _assemblyDetectionsRepo;

        public AssemblyDefectsAppService(IRepository<AssemblyDefects> assemblyDefectsRepo,
                                        IRepository<AssemblyDetection.AssemblyDetection> assemblyDetectionsRepo) : base(assemblyDefectsRepo)
        {
            _assemblyDefectsRepo = assemblyDefectsRepo;
            _assemblyDetectionsRepo = assemblyDetectionsRepo;
        }

    }
}
