using API.Data.Repositories;
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
        public UnitOfWork(IMapper mapper, UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext)
        {
            _mapper = mapper;
            _userManager = userManager;
            _dbContext = dbContext;
        }

        public ApplicationDbContext DbContext => _dbContext;
        public IUserRepository UserRepository => new UserRepository(_dbContext);
        public IProductRepository ProductRepository => new ProductRepository(_dbContext, _mapper);

        public async Task<bool> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}
