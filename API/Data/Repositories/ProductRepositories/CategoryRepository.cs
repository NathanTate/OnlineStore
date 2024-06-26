﻿using API.Interfaces;
using API.Models.DTO.ProductDTO;
using API.Models.DTO.ProductDTO.Requests;
using API.Models.DTO.ProductDTO.Responses;
using API.Models.ProductModel;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repositories.ProductRepositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        public CategoryRepository(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<Result<ProductCategoryResponse>> CreateCategoryAsync(ProductCategoryRequest model)
        {
            model.Id = 0;
            var category = _mapper.Map<ProductCategory>(model);

            if (_dbContext.ProductCategories.Any(p => p.CategoryName == category.CategoryName))
            {
                return Result.Fail("Category already exists");
            }

            await _dbContext.ProductCategories.AddAsync(category);

            return Result.Ok(_mapper.Map<ProductCategoryResponse>(category));
        }

        public async Task<IEnumerable<CategoriesResponse>> GetAllCategoryAsync()
        {
            var categories = await _dbContext.ProductCategories
                .Include(c => c.SubCategories).ToListAsync();

            var categoryResponses = categories.Select(category => new CategoriesResponse
            {
                Id = category.Id,
                CategoryName = category.CategoryName,
                CategoryDescription = category.CategoryDescription,
                SubcategoryGroups = category.SubCategories
                    .GroupBy(s => s.GroupName)
                    .Select(g => new SubcategoryGroupResponse
                    {
                        GroupName = g.Key,
                        Subcategories = _mapper.Map<IEnumerable<ProductSubCategoryDto>>(g.ToList())
                    })
                    .ToList()
            });

            return categoryResponses;

        }
        public async Task<Result<ProductCategoryResponse>> GetCategoryAsync(int id)
        {
            var category = await _dbContext.ProductCategories.FindAsync(id);  

            if (category == null)
            {
                return Result.Fail("Category doesn't exist");
            }

            await _dbContext.Entry(category).Collection(c => c.SubCategories).LoadAsync();

            return Result.Ok(_mapper.Map<ProductCategoryResponse>(category));
        }

        public async Task<Result> UpdateCategoryAsync(ProductCategoryRequest model)
        {
            var category = await _dbContext.ProductCategories.FindAsync(model.Id);

            if (category == null)
            {
                return Result.Fail("Category doesn't exist");
            }

            _mapper.Map(model, category);

            return Result.Ok();
        }

        public async Task<Result> DeleteCategoryAsync(int id)
        {
            var category = await _dbContext.ProductCategories.FindAsync(id);

            if (category == null)
            {
                return Result.Fail("Category doesn't exist");
            }

            _dbContext.ProductCategories.Remove(category);

            return Result.Ok();
        }


        public async Task<Result<ProductSubCategoryDto>> CreateSubCategoryAsync(ProductSubCategoryDto model)
        {
            model.Id = 0;
            var subCategory = _mapper.Map<ProductSubCategory>(model);

            if (_dbContext.ProductSubCategories.Any(p => p.SubCategoryName == subCategory.SubCategoryName))
            {
                return Result.Fail("SubCategory already exists");
            }

            if(!_dbContext.ProductCategories.Any(p => p.Id == subCategory.CategoryId))
            {
                return Result.Fail("Corresponding category doesn't exist");
            }


            await _dbContext.ProductSubCategories.AddAsync(subCategory);

            return Result.Ok(_mapper.Map<ProductSubCategoryDto>(subCategory));
        }

        public async Task<IEnumerable<SubcategoryGroupResponse>> GetAllSubCategoryAsync(int id = 1)
        {
            var groupedSubcategories = await _dbContext.ProductSubCategories
                .Where(c => c.CategoryId == id)
                .GroupBy(s => s.GroupName)
                .Select(g => new SubcategoryGroupResponse
                {
                    GroupName = g.Key,
                    Subcategories = _mapper.Map<IEnumerable<ProductSubCategoryDto>>(g.ToList())
                })
                .ToListAsync();

            return groupedSubcategories;
        }

        public async Task<Result<ProductSubCategoryDto>> GetSubCategoryAsync(int id)
        {
            var subCategory = await _dbContext.ProductSubCategories.FindAsync(id);

            if (subCategory == null)
            {
                return Result.Fail("Category doesn't exist");
            }

            //await _dbContext.Entry(subCategory).Collection(c => c.Products).LoadAsync();

            return Result.Ok(_mapper.Map<ProductSubCategoryDto>(subCategory));
        }

        public async Task<Result> UpdateSubCategoryAsync(ProductSubCategoryDto model)
        {

            var subCategory = await _dbContext.ProductSubCategories.FindAsync(model.Id);

            if (subCategory == null)
            {
                return Result.Fail("Category doesn't exist");
            }

            _mapper.Map(model, subCategory);

            return Result.Ok();
        }

        public async Task<Result> DeleteSubCategoryAsync(int id)
        {
            var category = await _dbContext.ProductSubCategories.FindAsync(id);

            if (category == null)
            {
                return Result.Fail("Category doesn't exist");
            }

            _dbContext.ProductSubCategories.Remove(category);

            return Result.Ok();
        }
    }
}
