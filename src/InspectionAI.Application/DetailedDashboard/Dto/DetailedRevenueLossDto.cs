using System;
using System.Collections.Generic;

namespace InspectionAI.DetailedDashboard.Dto
{
    public class DetailedRevenueLossDto
    {
        public List<DateTime> Labels { get; set; }
        public List<double> All { get; set; }
        public List<DetailedRevenueLossDataDto> Data { get; set; }
    }
}
