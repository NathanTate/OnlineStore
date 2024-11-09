using API.Extensions;
using API.Helpers;
using API.Helpers.RequestParams;
using API.Interfaces;
using API.Models.DTO.ProductDTO.Requests;
using API.Models.DTO.ProductDTO.Responses;
using API.Models.ProductModel;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CloudinaryDotNet.Actions;
using FluentResults;
using Microsoft.EntityFrameworkCore;
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

        public async Task<Result<ProductResponse>> CreateProductPlaceholderAsync()
        {
            Product placeholder = await _dbContext.Products.FirstOrDefaultAsync(x => x.IsPlaceholder);

            if (placeholder is null)
            {
                placeholder = new Product()
                {
                    IsPlaceholder = true,
                    SubCategoryId = 1,
                    BrandId = 1,
                    CreatedAt = DateTime.UtcNow
                };

                await _dbContext.Products.AddAsync(placeholder);
            };


            return Result.Ok(_mapper.Map<ProductResponse>(placeholder));
        }

        public async Task<PagedList<ProductResponse>> GetProductsAsync(ProductParams productParams)
        {
            IQueryable<Product> productsQuery = _dbContext.Products.AsQueryable();

            productsQuery = FilterProduct(productsQuery, productParams);

            if (productParams.SortBy?.ToLower() == "desc")
            {
                productsQuery = productsQuery.OrderByDescending(p => p.Quantity > 0).ThenByDescending(GetSortProperty(productParams));
            }
            else
            {
                productsQuery = productsQuery.OrderByDescending(p => p.Quantity > 0).ThenBy(GetSortProperty(productParams));
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

            if (product == null)
            {
                return Result.Fail("Product doesn't exist");
            }
            product.CategoryId = _dbContext.ProductSubCategories.Where(x => x.Id == product.SubCategoryId).Select(x => x.CategoryId).FirstOrDefault();
            var colors = await _dbContext.ProductColors.Where(pc => pc.ProductId == product.Id).Select(pc => pc.Color).ToListAsync();
            var productsDto = _mapper.Map<ProductResponse>(product);
            productsDto.Colors = _mapper.Map<List<ColorResponse>>(colors);

            return Result.Ok(productsDto);
        }

        public async Task<Result> UpdateProductAsync(ProductRequest model)
        {
            var product = await _dbContext.Products
                .Include(p => p.ProductSpecifications)
                .SingleOrDefaultAsync(p => p.Id == model.Id);

            if (product == null)
            {
                return Result.Fail("Product doesn't exist");
            }

            List<ProductColor> productColors = await _dbContext.ProductColors
                .Where(p => p.ProductId == product.Id)
                .Include(p => p.Color)
                .ToListAsync();

            _mapper.Map(model, product);

            List<Color> colors = new();

            foreach (var item in model.Colors)
            {
                var color = await _dbContext.Colors.FirstOrDefaultAsync(c => c.Value == item.ToLower());

                if (color == null)
                {
                    color = new Color
                    {
                        Value = item.ToLower(),
                    };

                    _dbContext.Colors.Add(color);
                }

                colors.Add(color);
            }

            foreach (var item in productColors)
            {
                if (!model.Colors.Any(color => color.ToLower() == item.Color.Value))
                {
                    _dbContext.ProductColors.Remove(item);
                }
            }

            var exisitingColorValues = colors.Select(c => c).ToHashSet();
            var newColors = colors.Where(color => !exisitingColorValues.Contains(color));

            foreach (var newColor in newColors)
            {
                var productColor = new ProductColor()
                {
                    ProductId = product.Id,
                    ColorId = newColor.Id,
                };

                _dbContext.ProductColors.Add(productColor);
            }


            product.IsPlaceholder = false;

            return Result.Ok();
        }

        public async Task<Result> UpdatePhotosAsync(PhotoUpdateRequest model)
        {
            var product = await _dbContext.Products.FindAsync(model.ItemId);

            if (product is null)
                return Result.Fail("Can't upload files to non exisiting product");

            await _dbContext.Entry(product)
            .Collection(p => p.ProductImages)
            .Query()
            .Where(i => model.IdsToRemove.Contains(i.Id))
            .LoadAsync();

            if (product.ProductImages.Count > 0)
            {
                _dbContext.ProductImages.RemoveRange(product.ProductImages);
                await _photoService.DeletePhotosAsync(product.ProductImages.Select(p => p.PublicId).ToList());
            }

            List<ImageUploadResult> result = await _photoService.UploadPhotosAsync(model.PhotoCollection);

            if (result is not null)
            {
                // bool defaultMain = false;

                // if (product.ProductImages.Count == 0)
                // {
                //     defaultMain = true;
                // }

                // foreach (var image in product.ProductImages)
                // {
                //     image.IsMain = false;
                // }

                foreach (var imageResult in result.WithIndex())
                {
                    product.ProductImages.Add(new ProductImage
                    {
                        Url = imageResult.item.SecureUrl.ToString(),
                        PublicId = imageResult.item.PublicId,
                        IsMain = false
                    });
                }
            }

            return Result.Ok();
        }

        public async Task<Result> SetMainPhotoAsync(SetMainPhotoRequest model)
        {
            var product = await _dbContext.Products.Include(p => p.ProductImages).SingleOrDefaultAsync(x => x.Id == model.itemId);

            if (product is null)
                return Result.Fail("Product doesn't exist");

            var photo = product.ProductImages.FirstOrDefault(p => p.Id == model.photoId);

            if (photo is null)
                return Result.Fail("photo doesn't exist");

            if (photo.IsMain)
                return Result.Fail("photo is already main");

            var currentMain = product.ProductImages.FirstOrDefault(p => p.IsMain);
            if (currentMain is not null) currentMain.IsMain = false;
            photo.IsMain = true;
            product.MainImageUrl = photo.Url;

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

        public async Task<IEnumerable<ColorResponse>> GetColorsAsync()
        {
            return _mapper.Map<IEnumerable<ColorResponse>>(await _dbContext.Colors.ToListAsync());
        }

        private static Expression<Func<Product, object>> GetSortProperty(ProductParams sortColumn)
        {
            Expression<Func<Product, object>> keySelector = sortColumn.SortColumn?.ToLower() switch
            {
                "rating" => product => product.ProductRating,
                "price" => product => product.SalePrice != 0 ? product.SalePrice : product.OriginalPrice,
                "originalprice" => product => product.OriginalPrice,
                "saleprice" => product => product.SalePrice,
                "quantity" => product => product.Quantity,
                "name" => product => product.Name,
                "date" => product => product.CreatedAt,
                _ => product => product.Id
            };

            return keySelector;
        }

        private static IQueryable<Product> FilterProduct(IQueryable<Product> productsQuery, ProductParams productParams)
        {
            if (productParams.categoryId != default(int))
            {
                productsQuery = productsQuery.Where(p => p.SubCategory.CategoryId == productParams.categoryId);
            }
            if (productParams.SubCategories.Count() != 0)
            {
                productsQuery = productsQuery.Where(p => productParams.SubCategories.Contains(p.SubCategoryId));
            }
            if (productParams.InStock == true)
            {
                productsQuery = productsQuery.Where(p => p.Quantity > 0);
            }
            if (productParams.InStock == false)
            {
                productsQuery = productsQuery.Where(p => p.Quantity <= 0);
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
