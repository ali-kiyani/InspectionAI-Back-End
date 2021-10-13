using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InspectionAI.Home.Dto
{
    public class RevenueLossDto
    {
        public List<DateTime> Labels { get; set; }
        public List<double> All { get; set; }
        public List<RevenueLossDataDto> Data { get; set; }
    }
}
