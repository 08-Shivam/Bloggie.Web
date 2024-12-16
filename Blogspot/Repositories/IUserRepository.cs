using Microsoft.AspNetCore.Identity;

namespace Blogspot.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<IdentityUser>> GetAll();
    }
}
