using System.Collections.Generic;

namespace InspectionAI.OverviewDashboard.Dto
{
    public class OverviewDefectRatioDto
    {
        public List<int> Ids { get; set; }
        public List<string> Name { get; set; }
        public List<int> Defects { get; set; }
        public List<int> Good { get; set; }
    }
}
