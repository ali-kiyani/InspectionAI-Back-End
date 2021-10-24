using System;
using System.Collections.Generic;

namespace InspectionAI.OverviewDashboard.Dto
{
    public class OverviewDefectTrendDto
    {
        public List<DateTime> Labels { get; set; }
        public List<int> All { get; set; }
        public List<OverviewDefectTrendDataDto> Data { get; set; }
    }
}
