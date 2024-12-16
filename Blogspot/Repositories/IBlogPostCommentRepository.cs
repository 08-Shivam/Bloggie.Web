using Blogspot.Models.Domain;

namespace Blogspot.Repositories
{
    public interface IBlogPostCommentRepository
    {
        Task<BlogPostComment> AddAsync(BlogPostComment blogPostComment);

        Task<IEnumerable<BlogPostComment>> GetCommentsByBlogIdAsync(Guid blogPostId); //Getting all comments

        Task<BlogPostComment> GetCommentByIdAsync(Guid id);

        Task<BlogPostComment?> UpdateCommentAsync(BlogPostComment blogPostComment); //Update the comment 

        Task<BlogPostComment?> DeleteAsync(Guid id);
    }
}
