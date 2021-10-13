using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InspectionAI.Home.Dto
{
    public class AssemblyDefects
    {
        public List<string> AssemblyNames { get; set; }
        public List<int> AssemblyId { get; set; }
        public List<int> AssemblyDefectsCount { get; set; }
    }
}
