using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InspectionAI.AssemblyDefects.Dto
{
    public class DefectiveProductsResponse
    {
        public string ImageUrl { get; set; }
        public DateTime DateTime { get; set; }
        public int AssemblyDetectionId { get; set; }
        public List<string> DefectNames { get; set; }
    }
}
