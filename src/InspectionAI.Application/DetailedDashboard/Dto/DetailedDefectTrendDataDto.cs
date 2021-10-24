using System;
using System.Collections.Generic;

namespace InspectionAI.DetailedDashboard.Dto
{
    public class DetailedDefectTrendDataDto
    {
        public String Name { get; set; }
        public int DefectId { get; set; }
        public List<int> Data { get; set; }
    }
}
