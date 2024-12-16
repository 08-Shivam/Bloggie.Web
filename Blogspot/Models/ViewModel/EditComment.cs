
namespace Blogspot.Models.ViewModel
{
    public class EditComment
    {
        public Guid Id { get; set; }

        public Guid BlogPostId { get; set; }

        public string Description { get; set; }

        public DateTime DateAdded { get; set; }

        public Guid CommentId { get; set; }
     }
}
