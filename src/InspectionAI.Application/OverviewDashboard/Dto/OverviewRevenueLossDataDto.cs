using System.Collections.Generic;

namespace InspectionAI.OverviewDashboard.Dto
{
    public class OverviewRevenueLossDataDto
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public List<double> Data { get; set; }
    }
}
