using Blogspot.Data;
using Blogspot.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Blogspot.Repositories
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly BlogspotDbContext blogspotDbContext;

        //Database interaction
        public BlogPostRepository(BlogspotDbContext blogspotDbContext)
        {
            this.blogspotDbContext = blogspotDbContext;
        }

        public async Task<BlogPost> AddAsync(BlogPost blogPost) //Add Blog
        {
            await blogspotDbContext.AddAsync(blogPost);
            await blogspotDbContext.SaveChangesAsync();
            return blogPost;
        }

        public async Task<BlogPost?> DeleteAsync(Guid id) //Delete blog
        {
            var existingBlog = await blogspotDbContext.BlogSpots.FindAsync(id);

            if (existingBlog != null) {
                blogspotDbContext.BlogSpots.Remove(existingBlog);
                await blogspotDbContext.SaveChangesAsync(); 
                return existingBlog;
            }
            else
            {
                return null;
            }
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync(string? searchQuery,
            string? sortBy, string? sortDirection, int pageNumber = 1, int pageSize = 100) //Get all blogs
        {
            var query = blogspotDbContext.BlogSpots.AsQueryable();
           //Filtering
            if (searchQuery != null && string.IsNullOrWhiteSpace(searchQuery) == false)
            {
                query = query.Where(x => x.Heading.Contains(searchQuery) || x.Tags.Any(x=>x.Name.Contains(searchQuery)));
            }

            //Sorting
            if (sortBy != null && string.IsNullOrWhiteSpace(sortBy) == false)
            {
                var isDesc = string.Equals(sortDirection, "Desc", StringComparison.OrdinalIgnoreCase);

                if (string.Equals(sortBy, "Heading", StringComparison.OrdinalIgnoreCase))
                {
                    query = isDesc ? query.OrderByDescending(x => x.Heading) : query.OrderBy(x => x.Heading);
                }

                if (string.Equals(sortBy, "Tags", StringComparison.OrdinalIgnoreCase))
                {
                    query = isDesc ? query.OrderByDescending(x => x.Tags) : query.OrderBy(x => x.Tags);
                }
            }

            //Pagination
            var skipResults = (pageNumber - 1) * pageSize;
            query = query.Skip(skipResults).Take(pageSize);

            return await query.Include(x=>x.Tags).ToListAsync();
            //return await blogspotDbContext.BlogSpots.Include(x=>x.Tags).ToListAsync();
        }

        public async Task<int> CountAsync()
        {
            return await blogspotDbContext.BlogSpots.CountAsync();
        }

        public async Task<BlogPost?> GetAsync(Guid id) // get by id
        {
            return await blogspotDbContext.BlogSpots.Include(x=>x.Tags).FirstOrDefaultAsync(x=>x.Id==id);
        }

        public async Task<BlogPost?> GetByUrlHandleAsync(string urlHandle)
        {
            return await blogspotDbContext.BlogSpots.Include(x=>x.Tags).
                FirstOrDefaultAsync(x=>x.UrlHandle==urlHandle);
        }

        public async Task<BlogPost?> UpdateAsync(BlogPost blogPost) //Update
        {
            var existingBlog=await blogspotDbContext.BlogSpots.Include(x=>x.Tags).FirstOrDefaultAsync(x=>x.Id==blogPost.Id);

            if (existingBlog != null) 
            {
                existingBlog.Id=blogPost.Id;
                existingBlog.Heading=blogPost.Heading;
                existingBlog.PageTitle=blogPost.PageTitle;
                existingBlog.PublishedDate=blogPost.PublishedDate;
                existingBlog.Author=blogPost.Author;
                existingBlog.ShortDescription=blogPost.ShortDescription;
                existingBlog.Content=blogPost.Content;
                existingBlog.FeaturedImageUrl=blogPost.FeaturedImageUrl;
                existingBlog.UrlHandle=blogPost.UrlHandle;
                existingBlog.Visible=blogPost.Visible;
                existingBlog.Tags=blogPost.Tags;

                await blogspotDbContext.SaveChangesAsync();
                return existingBlog;
            }
            return null;
        }
    }
}
