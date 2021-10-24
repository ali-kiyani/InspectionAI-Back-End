using Abp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InspectionAI.Product
{
    [Index(nameof(Name))]

    [Table("Product")]
    public class Product : Entity<int>
    {
        public string Name { get; set; }

        public virtual ICollection<Stage.Stage> Stages { get; set; }
    }
}
