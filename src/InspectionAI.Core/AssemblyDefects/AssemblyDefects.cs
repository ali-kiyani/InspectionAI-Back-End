using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InspectionAI.AssemblyDefects
{
    [Table("AssemblyDefects")]
    public class AssemblyDefects : Entity<int>
    {
        public int AssemblyDetectionId { get; set; }

        [ForeignKey("AssemblyDetectionId")]
        public AssemblyDetection.AssemblyDetection AssemblyDetection { get; set; }
        public int DefectId { get; set; }

        [ForeignKey("DefectId")]
        public Defects.Defects Defect { get; set; }
        public float Confidence { get; set; }
        public string ImageUrl { get; set; }
        public int StageId { get; set; }

        [ForeignKey("StageId")]
        public Stage.Stage Stage { get; set; }
        public DateTime DetectionTime { get; set; }
    }
}
