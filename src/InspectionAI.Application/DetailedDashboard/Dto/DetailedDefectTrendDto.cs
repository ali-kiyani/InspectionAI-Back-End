using System;
using System.Collections.Generic;

namespace InspectionAI.DetailedDashboard.Dto
{
    public class DetailedDefectTrendDto
    {
        public List<DateTime> Labels { get; set; }
        public List<int> All { get; set; }
        public List<DetailedDefectTrendDataDto> Data { get; set; }
    }
}
