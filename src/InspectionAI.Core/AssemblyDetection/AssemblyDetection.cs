using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InspectionAI.AssemblyDetection
{
    [Table("AssemblyDetection")]
    public class AssemblyDetection : Entity<int>
    {
        public int AssemblyLineId { get; set; }

        [ForeignKey("AssemblyLineId")]
        public AssemblyLine.AssemblyLine AssemblyLine { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product.Product Product { get; set; }
        public int StageId { get; set; }

        [ForeignKey("StageId")]
        public Stage.Stage Stage { get; set; }
        public DateTime DetectionTime { get; set; }
        public int DefectsCount { get; set; }
        public virtual ICollection<AssemblyDefects.AssemblyDefects> AssemblyDefects { get; set; }

    }
}
