using Abp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace InspectionAI.AssemblyDefects
{
    [Index(nameof(DetectionTime), nameof(Confidence))]
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
        public int StageId { get; set; }

        [ForeignKey("StageId")]
        public Stage.Stage Stage { get; set; }
        public DateTime DetectionTime { get; set; }
    }
}
