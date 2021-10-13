using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InspectionAI.Home.Dto
{
    public class ProductDefectTrendDataDto
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public List<int> Data { get; set; }
    }
}
