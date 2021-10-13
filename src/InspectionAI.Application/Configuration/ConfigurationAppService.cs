using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using InspectionAI.Configuration.Dto;

namespace InspectionAI.Configuration
{
    [AbpAuthorize]
    public class ConfigurationAppService : InspectionAIAppServiceBase, IConfigurationAppService
    {
        public async Task ChangeUiTheme(ChangeUiThemeInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
        }
    }
}
