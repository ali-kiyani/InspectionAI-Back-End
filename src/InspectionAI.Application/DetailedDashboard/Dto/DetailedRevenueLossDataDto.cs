using System.Collections.Generic;

namespace InspectionAI.DetailedDashboard.Dto
{
    public class DetailedRevenueLossDataDto
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public List<double> Data { get; set; }
    }
}
