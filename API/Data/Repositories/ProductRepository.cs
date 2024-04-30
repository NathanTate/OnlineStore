using API.Interfaces;
using API.Models.DTO.ProductDTO;
using API.Models.DTO.ProductDTO.Requests;
using API.Models.Product;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        public ProductRepository(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<ProductCategoryResponse> CreateCategory(ProductCategoryResponse model)
        {
            var category = _mapper.Map<ProductCategory>(model);

            if (_dbContext.ProductCategories.Any(p => p.CategoryName == category.CategoryName)) 
            {
                
            }

            await _dbContext.ProductCategories.AddAsync(category);

            return _mapper.Map<ProductCategoryResponse>(category);
        }

        public async Task<ProductSubCategoryDto> CreateSubCategory(ProductSubCategoryDto model)
        {
            var subCategory = _mapper.Map<ProductSubCategory>(model);
            await _dbContext.ProductSubCategories.AddAsync(subCategory);

            return _mapper.Map<ProductSubCategoryDto>(subCategory);
        }

        public async Task<IEnumerable<ProductCategoryResponse>> GetAllCategory()
        {
            return await _dbContext.ProductCategories
                .ProjectTo<ProductCategoryResponse>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public Task<ProductSubCategoryDto> GetAllSubCategory()
        {
            throw new NotImplementedException();
        }

        public async Task<ProductCategoryResponse> GetCategory(int categoryId)
        {
            return _mapper.Map<ProductCategoryResponse>(await _dbContext.ProductCategories
                .Where(x => x.Id == categoryId).FirstOrDefaultAsync());
        }
    }
}
