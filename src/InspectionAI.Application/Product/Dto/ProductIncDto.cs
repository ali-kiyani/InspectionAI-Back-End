using InspectionAI.Stage.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InspectionAI.Product.Dto
{
    public class ProductIncDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string ModelPath { get; set; }
        public List<StageIncDto> Stages { get; set; }

        public ProductIncDto()
        {
            Stages = new();
        }
    }
}
