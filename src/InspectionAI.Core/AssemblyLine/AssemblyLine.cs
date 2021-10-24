using Abp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InspectionAI.AssemblyLine
{
    [Table("AssemblyLine")]
    public class AssemblyLine : Entity<int>
    {
        public string Name { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product.Product Product { get; set; }
        public int StageId { get; set; }

        [ForeignKey("StageId")]
        public Stage.Stage Stage { get; set; }
    }
}
