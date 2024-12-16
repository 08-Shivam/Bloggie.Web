using Blogspot.Models.Domain;

namespace Blogspot.Repositories
{
    public interface IBlogPostRepository

    {
        Task<IEnumerable<BlogPost>> GetAllAsync(
            string? searchQuery = null,
            string? sortBy = null,
            string? sortDirection = null,
            int pageNumber = 1, int pageSize = 100); //Get all BlogPosts

        Task<BlogPost?> GetAsync(Guid id); //Get by id

        Task<BlogPost?> GetByUrlHandleAsync(string urlHandle);

        Task<BlogPost> AddAsync(BlogPost blogPost); //Add
         
        Task<BlogPost?> UpdateAsync(BlogPost blogPost); //Update

        Task<BlogPost?> DeleteAsync(Guid id); //Delete

        Task<int> CountAsync();

    }
}
