using Blogspot.Data;
using Blogspot.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Blogspot.Repositories
{
    public class BlogPostCommentRepository : IBlogPostCommentRepository
    {
        private readonly BlogspotDbContext blogspotDbContext;

        public BlogPostCommentRepository(BlogspotDbContext blogspotDbContextS)
        {
            this.blogspotDbContext = blogspotDbContextS;
        }

        public async Task<BlogPostComment> AddAsync(BlogPostComment blogPostComment)
        {
            await blogspotDbContext.BlogPostComment.AddAsync(blogPostComment);
            await blogspotDbContext.SaveChangesAsync();
            return blogPostComment;
        }

        public async Task<IEnumerable<BlogPostComment>> GetCommentsByBlogIdAsync(Guid blogPostId)
        {
            return await blogspotDbContext.BlogPostComment.Where(x=>x.BlogPostId == blogPostId).ToListAsync();
        }

        //Get comment by id
        public async Task<BlogPostComment> GetCommentByIdAsync(Guid id)
        {
            return await blogspotDbContext.BlogPostComment.FirstOrDefaultAsync(x => x.Id == id);
        }

        //Update comment by id
        public async Task<BlogPostComment?> UpdateCommentAsync(BlogPostComment blogPostComment)
        {
           
            var existingComment = await blogspotDbContext.BlogPostComment.FindAsync(blogPostComment.Id);
            if (existingComment != null)
            {
                existingComment.Description = blogPostComment.Description;
                existingComment.DateAdded = blogPostComment.DateAdded;

                await blogspotDbContext.SaveChangesAsync();
                return existingComment;
            }
            return null;
        }

        //Delete comment by selected id
        public async Task<BlogPostComment?> DeleteAsync(Guid id)
        {
            var existingComment = await blogspotDbContext.BlogPostComment.FindAsync(id);
            if (existingComment != null)
            {
                blogspotDbContext.BlogPostComment.Remove(existingComment);
                await blogspotDbContext.SaveChangesAsync();

                //Show deleted successfully
                return existingComment;
            }
            return null;
        }
    }
}
    