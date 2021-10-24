using Abp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InspectionAI.StageDefects
{
    [Table("StageDefects")]
    public class StageDefects : Entity<int>
    {
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product.Product Product { get; set; }
        public int StageId { get; set; }

        [ForeignKey("StageId")]
        public Stage.Stage Stage { get; set; }
        public int DefectId { get; set; }

        [ForeignKey("DefectId")]
        public Defects.Defects Defects { get; set; }
    }
}
