using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using InspectionAI.Authorization;

namespace InspectionAI
{
    [DependsOn(
        typeof(InspectionAICoreModule), 
        typeof(AbpAutoMapperModule))]
    public class InspectionAIApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<InspectionAIAuthorizationProvider>();
        }

        public override void Initialize()
        {
            var thisAssembly = typeof(InspectionAIApplicationModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddMaps(thisAssembly)
            );
        }
    }
}
