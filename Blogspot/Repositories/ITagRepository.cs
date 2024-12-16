using Blogspot.Models.Domain;

namespace Blogspot.Repositories
{
    public interface ITagRepository
    {
        //For searching/getting/retrieving tags based on search input
        Task<IEnumerable<Tag>>
            GetAllAsync(string? searchQuery = null,
            string? sortBy = null,
            string? sortDirection = null,
            int pageNumber = 1, int pageSize = 100); //getting all tags

        Task<Tag> GetAsync(Guid id); //for getting single id

        Task<Tag> AddAsync(Tag tag);

        Task<Tag?> UpdateAsync(Tag tag);

        Task<Tag?> DeleteAsync(Guid id);

        Task<int> CountAsync();
    }
}
