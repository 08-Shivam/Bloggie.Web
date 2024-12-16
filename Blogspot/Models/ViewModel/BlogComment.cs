namespace Blogspot.Models.ViewModel
{
    public class BlogComment
    {
        //It is used in comments view scrollable 

        public string Description {  get; set; }
        public DateTime DateAdded { get; set; } 
        public string Username {  get; set; }   
        public bool IsCurrentUser { get; set; }
        public Guid CommentId { get; set; }
    }
}
