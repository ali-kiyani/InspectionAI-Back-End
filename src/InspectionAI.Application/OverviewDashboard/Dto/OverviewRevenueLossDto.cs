using System;
using System.Collections.Generic;

namespace InspectionAI.OverviewDashboard.Dto
{
    public class OverviewRevenueLossDto
    {
        public List<DateTime> Labels { get; set; }
        public List<double> All { get; set; }
        public List<OverviewRevenueLossDataDto> Data { get; set; }
    }
}
