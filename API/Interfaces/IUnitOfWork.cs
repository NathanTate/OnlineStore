using API.Data;

namespace API.Interfaces
{
    public interface IUnitOfWork
    {
        public ApplicationDbContext DbContext { get; }
        public IUserRepository UserRepository { get; }
        public ICategoryRepository CategoryRepository { get; }
        public IProductRepository ProductRepository { get; }
        public IReviewRepository ReviewRepository { get; }
        Task<bool> SaveChangesAsync();
    }
}
