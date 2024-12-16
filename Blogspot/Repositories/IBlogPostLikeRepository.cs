using Blogspot.Models.Domain;

namespace Blogspot.Repositories
{
    public interface IBlogPostLikeRepository
    {
        Task<int> GetTotalLikes(Guid blogPostId);

        Task<IEnumerable<BlogPostLike>> GetLikesForBlog(Guid blogPostId);

        Task<BlogPostLike> AddLikeForBlog(BlogPostLike blogPostLike);

        //Task<BlogPostLike> DeleteLikeForBlog(Guid blogPostId);
    }
}
