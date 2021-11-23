using InspectionAI.AssemblyLine.Dto;
using InspectionAI.Defects.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InspectionAI.Stage.Dto
{
    public class StageIncDto
    {
        public int StageId { get; set; }
        public string Name { get; set; }
        public List<DefectsDto> Defects { get; set; }
        public List<AssemblyLineIncDto> AssemblyLines { get; set; }

        public StageIncDto()
        {
            Defects = new();
            AssemblyLines = new();
        }
    }
}
