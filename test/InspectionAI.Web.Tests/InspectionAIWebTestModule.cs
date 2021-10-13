using Abp.AspNetCore;
using Abp.AspNetCore.TestBase;
using Abp.Modules;
using Abp.Reflection.Extensions;
using InspectionAI.EntityFrameworkCore;
using InspectionAI.Web.Startup;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace InspectionAI.Web.Tests
{
    [DependsOn(
        typeof(InspectionAIWebMvcModule),
        typeof(AbpAspNetCoreTestBaseModule)
    )]
    public class InspectionAIWebTestModule : AbpModule
    {
        public InspectionAIWebTestModule(InspectionAIEntityFrameworkModule abpProjectNameEntityFrameworkModule)
        {
            abpProjectNameEntityFrameworkModule.SkipDbContextRegistration = true;
        } 
        
        public override void PreInitialize()
        {
            Configuration.UnitOfWork.IsTransactional = false; //EF Core InMemory DB does not support transactions.
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(InspectionAIWebTestModule).GetAssembly());
        }
        
        public override void PostInitialize()
        {
            IocManager.Resolve<ApplicationPartManager>()
                .AddApplicationPartsIfNotAddedBefore(typeof(InspectionAIWebMvcModule).Assembly);
        }
    }
}