using API.Helpers.RequestParams;
using API.Interfaces;
using API.Models.DTO.ProductDTO.Requests;
using API.Models.DTO.ProductDTO.Responses;
using API.Models.Product;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentResults;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

namespace API.Data.Repositories.ProductRepositories
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

        public async Task<Result<ProductResponse>> CreateProductAsync(ProductRequest model)
        {
            var product = _mapper.Map<Product>(model);

            await _dbContext.Products.AddAsync(product);

            return Result.Ok(_mapper.Map<ProductResponse>(product));
        }

        public async Task<IEnumerable<ProductResponse>> GetProductsAsync(ProductParams productParams)
        {
            IQueryable<Product> products = _dbContext.Products.AsQueryable()
                .Where(p => p.SubCategory.CategoryId == productParams.categoryId)
                                .Include(p => p.ProductItems);

            if(productParams.subCategoryIds.Count() != 0)
            {
                products = products.Where(p => productParams.subCategoryIds.Contains(p.SubCategoryId));
            }
            if(productParams.BrandId != default(int))
            {
                products = products.Where(p => p.BrandId == productParams.BrandId);
            }
            if (productParams.Colors.Count() != 0)
            {
                products = products.Where(p => p.ProductItems.Where(b => productParams.Colors.Contains(b.Color)).Any());
            }
            if (productParams.PriceMax != default(long))
            {
                products = products.Where(p => p.ProductItems.Any(p => p.OriginalPrice <= productParams.PriceMax));
            }
            if (productParams.PriceMin != default(long))
            {
                products = products.Where(p => p.ProductItems.Any(p => p.OriginalPrice >= productParams.PriceMin));
            }
            var result = await products.AsNoTracking().ProjectTo<ProductResponse>(_mapper.ConfigurationProvider).ToListAsync();
            return result;
        }

        public async Task<Result<ProductResponse>> GetProductAsync(int id)
        {
            var product = await _dbContext.Products.FindAsync(id);

            if(product == null) 
            {
                return Result.Fail("Product doesn't exist");
            }

            await _dbContext.Entry(product).Collection(p => p.ProductItems).LoadAsync();

            return Result.Ok(_mapper.Map<ProductResponse>(product));
        }

        public async Task<Result> UpdateProductAsync(ProductRequest model)
        {
            var product = await _dbContext.Products.FindAsync(model.Id);

            if(product == null)
            {
                return Result.Fail("Product doesn't exist");
            }

            _mapper.Map(model, product);

            return Result.Ok();
        }

        public async Task<Result> DeleteProductAsync(int id)
        {
            var product = await _dbContext.Products.FindAsync(id);

            if(product == null)
            {
                return Result.Fail("Product doesn't exist");
            }
            _dbContext.Products.Remove(product);

            return Result.Ok();
        }

    }
}
