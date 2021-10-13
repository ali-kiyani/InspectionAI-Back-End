using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using AutoMapper;
using InspectionAI.Product.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InspectionAI.Product
{
    public class ProductAppService : IProductAppService
    {
        private readonly IRepository<Product> _productRepo;

        public ProductAppService(IRepository<Product> productRepo)
        {
            _productRepo = productRepo;
        }

        public async Task<ListResultDto<ProductDto>> GetAllProducts()
        {
            var config = new MapperConfiguration(cfg =>
                    cfg.CreateMap<Product, ProductDto>()
                );
            var mapper = new Mapper(config);
            return new ListResultDto<ProductDto>(mapper.Map<List<ProductDto>>(await _productRepo.GetAllListAsync()));
        }
    }
}
