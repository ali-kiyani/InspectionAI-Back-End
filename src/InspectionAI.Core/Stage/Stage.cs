using Abp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InspectionAI.Stage
{
    [Index(nameof(Name), nameof(Cost))]

    [Table("Stage")]
    public class Stage : Entity<int>
    {
        public string Name { get; set; }
        public float Cost { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product.Product Product { get; set; }
        public virtual ICollection<AssemblyLine.AssemblyLine> AssemblyLines { get; set; }
    }
}
