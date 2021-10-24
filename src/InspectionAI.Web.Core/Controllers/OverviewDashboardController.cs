using InspectionAI.OverviewDashboard;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InspectionAI.Controllers
{
    [Route("api/[controller]/[action]")]

    public class OverviewDashboardController : InspectionAIControllerBase
    {
        private readonly IOverviewDashboardAppService _overviewDashboardService;

        public OverviewDashboardController(IOverviewDashboardAppService overviewDashboardService)
        {
            _overviewDashboardService = overviewDashboardService;
        }
    }
}
