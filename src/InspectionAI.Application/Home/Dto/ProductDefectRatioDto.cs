using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InspectionAI.Home.Dto
{
    public class ProductDefectRatioDto
    {
        public List<int> Ids { get; set; }
        public List<string> Name { get; set; }
        public List<int> Defects { get; set; }
        public List<int> Good { get; set; }
    }
}
