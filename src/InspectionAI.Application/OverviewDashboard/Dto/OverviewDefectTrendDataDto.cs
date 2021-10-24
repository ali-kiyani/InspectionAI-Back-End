using System.Collections.Generic;

namespace InspectionAI.OverviewDashboard.Dto
{
    public class OverviewDefectTrendDataDto
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public List<int> Data { get; set; }
    }
}
