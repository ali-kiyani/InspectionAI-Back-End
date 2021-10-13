using InspectionAI.Home;
using InspectionAI.Home.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InspectionAI.Controllers
{
    [Route("api/[controller]/[action]")]
    public class HomeController : InspectionAIControllerBase
    {
        private readonly IHomeAppService _homeAppService;

        public HomeController(IHomeAppService homeAppService)
        {
            _homeAppService = homeAppService;
        }
    }
}
