using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using AutoMapper;
using InspectionAI.Product.Dto;
using Microsoft.EntityFrameworkCore;
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
        private readonly IRepository<StageDefects.StageDefects> _stageDefectsRepo;

        public ProductAppService(IRepository<Product> productRepo, IRepository<StageDefects.StageDefects> stageDefectsRepo)
        {
            _productRepo = productRepo;
            _stageDefectsRepo = stageDefectsRepo;
        }

        public async Task<ListResultDto<ProductDto>> GetAllProducts()
        {
            var config = new MapperConfiguration(cfg =>
                    cfg.CreateMap<Product, ProductDto>()
                );
            var mapper = new Mapper(config);
            return new ListResultDto<ProductDto>(mapper.Map<List<ProductDto>>(await _productRepo.GetAllListAsync()));
        }

        public Task<ProductHeirarchyDto> GetAllProductsHeirarchy()
        {
            ProductHeirarchyDto pDto = new();
            var productData = _productRepo.GetAll().Include(x => x.Stages).ThenInclude(x => x.AssemblyLines).ToList();
            var defects = _stageDefectsRepo.GetAll().Include(x => x.Defects).ToList();
            foreach (var p in productData)
            {
                ProductIncDto pIncDto = new()
                {
                    ProductId = p.Id,
                    Name = p.Name,
                    ModelPath = p.ModelPath,
                    Stages = p.Stages.Where(st => st.ProductId == p.Id).Select(x => new Stage.Dto.StageIncDto
                    {
                        StageId = x.Id,
                        Name = x.Name,
                        AssemblyLines = x.AssemblyLines.Where(s => s.ProductId == p.Id && s.StageId == x.Id).Select(s => new AssemblyLine.Dto.AssemblyLineIncDto
                        {
                            Name = s.Name,
                            Id = s.Id
                        }).ToList(),
                        Defects = defects.Where(d => d.ProductId == p.Id && d.StageId == x.Id).Select(d => new Defects.Dto.DefectsDto
                        {
                            Id = d.DefectId,
                            Name = d.Defects.Name
                        }).ToList()
                    }).ToList()
                };
                pDto.Products.Add(pIncDto);
            }
            return Task.FromResult(pDto);
        }
    }
}
