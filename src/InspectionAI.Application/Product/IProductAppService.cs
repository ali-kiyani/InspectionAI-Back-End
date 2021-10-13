using Abp.Application.Services;
using Abp.Application.Services.Dto;
using InspectionAI.Product.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InspectionAI.Product
{
    public interface IProductAppService : IApplicationService
    {
        Task<ListResultDto<ProductDto>> GetAllProducts();
    }
}
