using API.Helpers;
using API.Helpers.RequestParams;
using API.Interfaces;
using API.Models.DTO.ProductDTO;
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
using System.Linq.Expressions;
using Color = API.Models.ProductModel.Color;

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
            Result<List<ImageUploadResult>> results = await _photoService.UploadPhotoAsync(model.ProductImages);

            if (results.IsFailed)
            {
                return Result.Fail(results.Errors[0]);
            }
            model.Id = 0;

            var product = _mapper.Map<Product>(model);
            product.CreatedAt = DateTime.UtcNow;

            List<ProductColor> productColors = new();
            foreach (var result in results.Value)
            {
                product.ProductImages.Add(new ProductImage
                {
                    Url = result.SecureUrl.ToString(),
                    PublicId = result.PublicId,
                    IsMain = model.IsMainImage
                });
            }

            foreach (var value in model.Colors.Select(c => c.Value))
            {
                var color = await _dbContext.Colors.FirstOrDefaultAsync(c => c.Value == value.ToLower());

                if (color == null)
                {
                    color = new Color
                    {
                        Value = value.ToLower(),
                    };

                    _dbContext.Colors.Add(color);
                }

                productColors.Add(new ProductColor { Color = color });
            }

            await _dbContext.Products.AddAsync(product);

            foreach (var color in productColors)
            {
                color.Product = product;
                _dbContext.ProductColors.Add(color);
            }

            return Result.Ok(_mapper.Map<ProductResponse>(product));
        }

        public async Task<PagedList<ProductResponse>> GetProductsAsync(ProductParams productParams)
        {
            IQueryable<Product> productsQuery = _dbContext.Products.AsQueryable();

            productsQuery = FilterProduct(productsQuery, productParams);

            if (productParams.SortBy?.ToLower() == "desc")
            {
                productsQuery = productsQuery.OrderByDescending(GetSortProperty(productParams));
            }
            else
            {
                productsQuery = productsQuery.OrderBy(GetSortProperty(productParams));
            }

            var result = await PagedList<ProductResponse>.CreateAsync(
                productsQuery.AsNoTracking().ProjectTo<ProductResponse>(_mapper.ConfigurationProvider),
                productParams.Page,
                productParams.PageSize);

            return result;
        }

        public async Task<Result<ProductResponse>> GetProductAsync(int id)
        {
            IQueryable<Product> productQuery = _dbContext.Products.AsQueryable().AsNoTracking().Where(x => x.Id == id);
            var product = await productQuery.ProjectTo<ProductResponse>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();
            product.CategoryId = _dbContext.ProductSubCategories.Where(x => x.Id == product.SubCategoryId).Select(x => x.CategoryId).FirstOrDefault();

            if (product == null)
            {
                return Result.Fail("Product doesn't exist");
            }
            var colors = await _dbContext.ProductColors.Where(pc => pc.ProductId == product.Id).Select(pc => pc.Color).ToListAsync();
            var productsDto = _mapper.Map<ProductResponse>(product);
            productsDto.Colors = _mapper.Map<List<ColorDto>>(colors);

            return Result.Ok(productsDto);
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

        public async Task<IEnumerable<ColorDto>> GetColorsAsync()
        {
            return _mapper.Map<IEnumerable<ColorDto>>(await _dbContext.Colors.ToListAsync());
        }

        private static Expression<Func<Product, object>> GetSortProperty(ProductParams sortColumn)
        {
            Expression<Func<Product, object>> keySelector = sortColumn.SortColumn?.ToLower() switch
            {
                "rating" => product => product.ProductRating,
                "price" => product => product.OriginalPrice,
                "date" => product => product.CreatedAt,
                _ => product => product.Id
            };

            return keySelector;
        }

        private static IQueryable<Product> FilterProduct(IQueryable<Product> productsQuery, ProductParams productParams)
        {
            if(productParams.categoryId != default(int))
            {
                productsQuery =  productsQuery.Where(p => p.SubCategory.CategoryId == productParams.categoryId);
            }
            if (productParams.subCategories.Count() != 0)
            {
                productsQuery = productsQuery.Where(p => productParams.subCategories.Contains(p.SubCategoryId));
            }
            if (productParams.BrandId != default(int))
            {
                productsQuery = productsQuery.Where(p => p.BrandId == productParams.BrandId);
            }
            if (productParams.Colors.Count() != 0)
            {
                productsQuery = productsQuery.Where(p => productParams.Colors.Intersect(p.Colors.Select(x => x.Color.Value)).Any());
            }
            if (productParams.PriceMax != default(decimal))
            {
                productsQuery = productsQuery.Where(p => p.OriginalPrice <= productParams.PriceMax);
            }
            if (productParams.PriceMin != default(decimal))
            {
                productsQuery = productsQuery.Where(p => p.OriginalPrice >= productParams.PriceMin);
            }
            if (!string.IsNullOrWhiteSpace(productParams.SearchTerm)) 
            {
                productsQuery = productsQuery
                .Where(p => p.Name.ToLower().Contains(productParams.SearchTerm.ToLower()) ||
                p.Id.ToString().Contains(productParams.SearchTerm) ||
                p.Brand.BrandName.ToLower().Contains(productParams.SearchTerm) ||
                p.Description.ToLower().Contains(productParams.SearchTerm));
            }

            return productsQuery;
        }

    }
}
