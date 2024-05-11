using API.Data.Repositories.CartRepository;
using API.Data.Repositories.CouponRepositories;
using API.Data.Repositories.OrderRepositories;
using API.Data.Repositories.ProductRepositories;
using API.Data.Repositories.UserRepositories;
using API.Interfaces;
using API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace API.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly IPhotoService _photoService;
        public UnitOfWork(IMapper mapper, UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext, IPhotoService photoService)
        {
            _mapper = mapper;
            _userManager = userManager;
            _dbContext = dbContext;
            _photoService = photoService;
        }

        public ApplicationDbContext DbContext => _dbContext;
        public IUserRepository UserRepository => new UserRepository(_dbContext);
        public ICategoryRepository CategoryRepository => new CategoryRepository(_dbContext, _mapper);
        public IProductRepository ProductRepository => new ProductRepository(_dbContext, _mapper, _photoService);
        public IReviewRepository ReviewRepository => new ReviewRepository(_dbContext, _mapper);
        public ICouponRepository CouponRepository => new CouponRepository(_dbContext, _mapper);
        public ICartRepository CartRepository => new CartRepository(_dbContext, _mapper);
        public IOrderRepository OrderRepository => new OrderRepository(_dbContext, _mapper, _userManager);

        public async Task<bool> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}
