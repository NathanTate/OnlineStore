using API.Interfaces;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public UserRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<ApplicationUser> GetById(string id)
        {
            return await _dbContext.Users.SingleOrDefaultAsync(u => u.Id == id); 
        }
    }
}
