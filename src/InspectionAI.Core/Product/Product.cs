using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InspectionAI.Product
{
    [Table("Product")]
    public class Product : Entity<int>
    {
        public string Name { get; set; }

        public virtual ICollection<Stage.Stage> Stages { get; set; }
    }
}
