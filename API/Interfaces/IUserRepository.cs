using API.Models;

namespace API.Interfaces
{
    public interface IUserRepository
    {
        Task<ApplicationUser> GetById(string id);
    }
}
