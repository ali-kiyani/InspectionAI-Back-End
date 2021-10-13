using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using InspectionAI.Configuration;

namespace InspectionAI.Web.Host.Startup
{
    [DependsOn(
       typeof(InspectionAIWebCoreModule))]
    public class InspectionAIWebHostModule: AbpModule
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public InspectionAIWebHostModule(IWebHostEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(InspectionAIWebHostModule).GetAssembly());
        }
    }
}
