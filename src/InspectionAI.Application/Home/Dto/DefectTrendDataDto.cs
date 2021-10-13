using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InspectionAI.Home.Dto
{
    public class DefectTrendDataDto
    {
        public String Name { get; set; }
        public int DefectId { get; set; }
        public List<int> Data { get; set; }
    }
}
