using Abp.Authorization;
using InspectionAI.Authorization.Roles;
using InspectionAI.Authorization.Users;

namespace InspectionAI.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}
