using InspectionAI.DetailedDashboard;
using Microsoft.AspNetCore.Mvc;

namespace InspectionAI.Controllers
{
    [Route("api/[controller]/[action]")]
    public class DetailedDashboardController : InspectionAIControllerBase
    {
        private readonly IDetailedDashboardAppService _detailedDashboardService;

        public DetailedDashboardController(IDetailedDashboardAppService detailedDashboardService)
        {
            _detailedDashboardService = detailedDashboardService;
        }
    }
}
