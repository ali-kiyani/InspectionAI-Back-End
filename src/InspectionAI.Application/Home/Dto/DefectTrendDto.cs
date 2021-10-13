using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InspectionAI.Home.Dto
{
    public class DefectTrendDto
    {
        public List<DateTime> Labels { get; set; }
        public List<int> All { get; set; }
        public List<DefectTrendDataDto> Data { get; set; }
    }
}
