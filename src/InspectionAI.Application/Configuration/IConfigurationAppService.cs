using System.Threading.Tasks;
using InspectionAI.Configuration.Dto;

namespace InspectionAI.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
