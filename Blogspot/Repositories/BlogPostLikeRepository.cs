using Blogspot.Data;
using Blogspot.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Blogspot.Repositories
{
    public class BlogPostLikeRepository : IBlogPostLikeRepository
    {
        private readonly BlogspotDbContext blogspotDbContext;

        public BlogPostLikeRepository(BlogspotDbContext blogspotDbContext)
        {
            this.blogspotDbContext = blogspotDbContext;
        }

        //Add Like
        public async Task<BlogPostLike> AddLikeForBlog(BlogPostLike blogPostLike)
        {
            await blogspotDbContext.BlogPostLike.AddAsync(blogPostLike);
            await blogspotDbContext.SaveChangesAsync();
            return blogPostLike;
        }

        //Delete Like
        //public Task<BlogPostLike> DeleteLikeForBlog(Guid blogPostId)
        //{
            
        //}

        //Get Like
        public async Task<IEnumerable<BlogPostLike>> GetLikesForBlog(Guid blogPostId)
        {
            return await blogspotDbContext.BlogPostLike.Where(x => x.BlogPostId == blogPostId).ToListAsync();
        }

        //Get Total Likes
        public async Task<int> GetTotalLikes(Guid blogPostId)
        {
            return await blogspotDbContext.BlogPostLike
                .CountAsync(x=>x.BlogPostId==blogPostId);
        }
    }
}
