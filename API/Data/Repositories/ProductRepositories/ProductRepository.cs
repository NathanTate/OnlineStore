using API.Helpers.RequestParams;
using API.Interfaces;
using API.Models.DTO.ProductDTO.Requests;
using API.Models.DTO.ProductDTO.Responses;
using API.Models.ProductModel;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CloudinaryDotNet.Actions;
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
        private readonly IPhotoService _photoService;
        public ProductRepository(ApplicationDbContext dbContext, IMapper mapper, IPhotoService photoService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _photoService = photoService;
        }

        public async Task<Result<ProductResponse>> CreateProductAsync(ProductRequest model)
        {
            Result<ImageUploadResult> result = await _photoService.UploadPhotoAsync(model.ProductImage);

            if(result.IsFailed)
            {
                return Result.Fail(result.Errors[0]);
            }
            model.Id = 0;

            var product = _mapper.Map<Product>(model);
            product.ProductImages.Add(new ProductImage
            {
                Url = result.Value.Url.ToString(),
                PublicId = result.Value.PublicId,
                IsMain = model.IsMainImage
            });

            await _dbContext.Products.AddAsync(product);

            return Result.Ok(_mapper.Map<ProductResponse>(product));
        }

        public async Task<IEnumerable<ProductResponse>> GetProductsAsync(ProductParams productParams)
        {
            IQueryable<Product> products = _dbContext.Products.AsQueryable()
                .Where(p => p.SubCategory.CategoryId == productParams.categoryId);


            if (productParams.subCategories.Count() != 0)
            {
                products = products.Where(p => productParams.subCategories.Contains(p.SubCategoryId));
            }
            if (productParams.BrandId != default(int))
            {
                products = products.Where(p => p.BrandId == productParams.BrandId);
            }
            if (productParams.Colors.Count() != 0)
            {
                products = products.Where(p => productParams.Colors.Contains(p.Color));
            }
            if (productParams.PriceMax != default(decimal))
            {
                products = products.Where(p => p.OriginalPrice <= productParams.PriceMax);
            }
            if (productParams.PriceMin != default(decimal))
            {
                products = products.Where(p => p.OriginalPrice >= productParams.PriceMin);
            }
            var result = await products.AsNoTracking().ProjectTo<ProductResponse>(_mapper.ConfigurationProvider).ToListAsync();
            return result;
        }

        public async Task<Result<ProductResponse>> GetProductAsync(int id)
        {
            var product = await _dbContext.Products.FindAsync(id);

            if (product == null)
            {
                return Result.Fail("Product doesn't exist");
            }

            return Result.Ok(_mapper.Map<ProductResponse>(product));
        }

        public async Task<Result> UpdateProductAsync(ProductRequest model)
        {
            var product = await _dbContext.Products.FindAsync(model.Id);

            if (product == null)
            {
                return Result.Fail("Product doesn't exist");
            }

            _mapper.Map(model, product);

            return Result.Ok();
        }

        public async Task<Result> DeleteProductAsync(int id)
        {
            var product = await _dbContext.Products.FindAsync(id);

            if (product == null)
            {
                return Result.Fail("Product doesn't exist");
            }
            _dbContext.Products.Remove(product);

            return Result.Ok();
        }

        public async Task<Result> DeletePhotoAsync(int productId, int photoId)
        {
            var photo = await _dbContext.ProductImages.FindAsync(photoId);

            if (photo == null)
            {
                return Result.Fail("Photo doesn't exist");
            }

            await _photoService.DeletePhotoAsync(photo.PublicId);
            _dbContext.ProductImages.Remove(photo);

            return Result.Ok();
        }

    }
}
