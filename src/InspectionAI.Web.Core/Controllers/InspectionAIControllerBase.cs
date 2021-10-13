using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;

namespace InspectionAI.Controllers
{
    public abstract class InspectionAIControllerBase: AbpController
    {
        protected InspectionAIControllerBase()
        {
            LocalizationSourceName = InspectionAIConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
