using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InspectionAI.Product.Dto
{
    [AutoMap(typeof(Product))]
    public class ProductDto : EntityDto
    {
        public string Name { get; set; }
    }
}
