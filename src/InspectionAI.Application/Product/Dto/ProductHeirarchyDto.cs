using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InspectionAI.Product.Dto
{
    public class ProductHeirarchyDto
    {
        public List<ProductIncDto> Products { get; set; }
        public ProductHeirarchyDto()
        {
            Products = new();
        }
    }
}
