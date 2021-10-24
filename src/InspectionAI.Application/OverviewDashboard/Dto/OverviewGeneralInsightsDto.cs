using System;
using System.Collections.Generic;

namespace InspectionAI.OverviewDashboard.Dto
{
    public class OverviewGeneralInsightsDto
    {
        public int TotalDetections { get; set; }
        public int TotalDefects { get; set; }
        public int TotalGood { get; set; }
        public List<DateTime>Labels { get; set; }
        public List<int> Detections { get; set; }
        public List<int> Defects { get; set; }
        public List<int> Good { get; set; }
    }
}
