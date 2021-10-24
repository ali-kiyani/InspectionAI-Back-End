using System.Collections.Generic;

namespace InspectionAI.DetailedDashboard.Dto
{
    public class DetailedAssemblyDefects
    {
        public List<string> AssemblyNames { get; set; }
        public List<int> AssemblyId { get; set; }
        public List<int> AssemblyDefectsCount { get; set; }
    }
}
