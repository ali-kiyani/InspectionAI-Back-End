using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InspectionAI.Defects
{
    [Table("Defects")]
    public class Defects : Entity<int>
    {
        public string Name { get; set; }
        public string Color { get; set; }
    }
}
