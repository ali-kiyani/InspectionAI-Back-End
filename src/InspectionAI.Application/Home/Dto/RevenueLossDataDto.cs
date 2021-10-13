using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InspectionAI.Home.Dto
{
    public class RevenueLossDataDto
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public List<double> Data { get; set; }
    }
}
