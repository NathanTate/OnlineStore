using API.Data;

namespace API.Interfaces
{
    public interface IUnitOfWork
    {
        public ApplicationDbContext DbContext { get; }
        public IUserRepository UserRepository { get; }
        public IProductRepository ProductRepository { get; }
        Task<bool> SaveChangesAsync();
    }
}
