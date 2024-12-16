using Blogspot.Data;
using Blogspot.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Blogspot.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly BlogspotDbContext blogspotDbContext;

        public TagRepository(BlogspotDbContext blogspotDbContext)
        {
            this.blogspotDbContext = blogspotDbContext;
        }

        //Save tags received from AdminTagController 
        public async Task<Tag> AddAsync(Tag tag)
        {
            await blogspotDbContext.Tags.AddAsync(tag);
            await blogspotDbContext.SaveChangesAsync();
            return tag;
        }

        //Get All Tag on home page
        public async Task<IEnumerable<Tag>> GetAllAsync(string? searchQuery,
            string? sortBy, string? sortDirection,int pageNumber=1, int pageSize=100)
        {
            var query = blogspotDbContext.Tags.AsQueryable();

            //Searching
            if (searchQuery != null && string.IsNullOrWhiteSpace(searchQuery)==false) 
            {
                query = query.Where(x => x.Name.Contains(searchQuery) ||x.DisplayName.Contains(searchQuery));
                
            }
            //Sorting
            if(sortBy!=null && string.IsNullOrWhiteSpace(sortBy)==false) 
            { 
                var isDesc= string.Equals(sortDirection,"Desc", StringComparison.OrdinalIgnoreCase);

                if (string.Equals(sortBy, "Name", StringComparison.OrdinalIgnoreCase))
                {
                    query = isDesc ? query.OrderByDescending(x => x.Name):query.OrderBy(x=>x.Name);
                }

                if (string.Equals(sortBy, "DisplayName", StringComparison.OrdinalIgnoreCase))
                {
                    query = isDesc ? query.OrderByDescending(x => x.DisplayName) : query.OrderBy(x => x.DisplayName);
                }
            }

            //Pagination
            var skipResults = (pageNumber-1)*pageSize;
            query=query.Skip(skipResults).Take(pageSize);

            return await query.ToListAsync();

            //return await blogspotDbContext.Tags.ToListAsync();
        }

        public async Task<int> CountAsync()
        {
            return await blogspotDbContext.Tags.CountAsync();
        }

        //Delete Tag
        public async Task<Tag?> DeleteAsync(Guid id)
        {
            var exitingTag = await blogspotDbContext.Tags.FindAsync(id);

            if (exitingTag != null)
            {
                blogspotDbContext.Tags.Remove(exitingTag);
                await blogspotDbContext.SaveChangesAsync();

                //Show deleted successfully
                return exitingTag;
            }
            return null;
        }

        //Get single tag by id
        public async Task<Tag> GetAsync(Guid id)
        {
            return await blogspotDbContext.Tags.FirstOrDefaultAsync(x=>x.Id==id);
        }

        //Update tag 
        public async Task<Tag?> UpdateAsync(Tag tag)
        {
            var existingtag=await blogspotDbContext.Tags.FindAsync(tag.Id);

            if (existingtag != null) 
            {
                existingtag.Name=tag.Name;
                existingtag.DisplayName=tag.DisplayName;

                await blogspotDbContext.SaveChangesAsync();

                return existingtag;
            }
            return null;            
        }
    }
}
