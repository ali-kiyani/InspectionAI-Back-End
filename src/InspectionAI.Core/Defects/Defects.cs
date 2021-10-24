using Abp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InspectionAI.Defects
{
    [Index(nameof(Name))]

    [Table("Defects")]
    public class Defects : Entity<int>
    {
        public string Name { get; set; }
        public string Color { get; set; }
    }
}
