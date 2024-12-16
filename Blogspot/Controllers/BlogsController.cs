using Blogspot.Models.Domain;
using Blogspot.Models.ViewModel;
using Blogspot.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Blogspot.Controllers
{
    
    public class BlogsController : Controller
    {
        private readonly IBlogPostRepository blogPostRepository;
        private readonly IBlogPostLikeRepository blogPostLikeRepository;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly UserManager<IdentityUser> userManager;
        private readonly IBlogPostCommentRepository blogPostCommentRepository;

        public BlogsController(IBlogPostRepository blogPostRepository, 
            IBlogPostLikeRepository blogPostLikeRepository,
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            IBlogPostCommentRepository blogPostCommentRepository)
        {
            this.blogPostRepository = blogPostRepository;
            this.blogPostLikeRepository = blogPostLikeRepository;
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.blogPostCommentRepository = blogPostCommentRepository;
        }

        [HttpGet]   
        public async Task<IActionResult> Index(string urlHandle)
        {
            var liked = false;
            var blogPost= await blogPostRepository.GetByUrlHandleAsync(urlHandle);
            var blogDetailsViewModel = new BlogDetailsViewModel();
            if (blogPost!=null) 
            {
                var totalLikes = await blogPostLikeRepository.GetTotalLikes(blogPost.Id);

                var userId = userManager.GetUserId(User);

                if (signInManager.IsSignedIn(User)) 
                {
                    //Get like for this user if liked
                    var likesForBlogs = await blogPostLikeRepository.GetLikesForBlog(blogPost.Id);


                    if (userId != null)
                    {
                        var likeFromUser= likesForBlogs.FirstOrDefault(x=>x.UserId==Guid.Parse(userId));
                        liked=likeFromUser!=null;   
                    }
                }

                //Get comments for blogpost
                var blogCommentsDomainModel= await blogPostCommentRepository.GetCommentsByBlogIdAsync(blogPost.Id);

                var blogCommentsForView = new List<BlogComment>();

                foreach (var blogComment in blogCommentsDomainModel) {
                    blogCommentsForView.Add(new BlogComment
                    {
                        CommentId=blogComment.Id,
                        Description=blogComment.Description,
                        DateAdded=blogComment.DateAdded,
                        Username=(await userManager.FindByIdAsync(blogComment.UserId.ToString())).UserName,
                        IsCurrentUser=blogComment.UserId.ToString() == userId
                    });
                }

                blogDetailsViewModel = new BlogDetailsViewModel
                {
                    Id = blogPost.Id,
                    Content = blogPost.Content,
                    PageTitle = blogPost.PageTitle,
                    Author=blogPost.Author,
                    FeaturedImageUrl = blogPost.FeaturedImageUrl,
                    Heading=blogPost.Heading,
                    PublishedDate=blogPost.PublishedDate,
                    ShortDescription=blogPost.ShortDescription,
                    UrlHandle=blogPost.UrlHandle,
                    Visible=blogPost.Visible,
                    Tags=blogPost.Tags,
                    TotalLikes=totalLikes,
                    Liked=liked,
                    Comments=blogCommentsForView
                };
            }
            return View(blogDetailsViewModel);
        }

        //add comment
        [HttpPost]
        public async Task<IActionResult> Index(BlogDetailsViewModel blogDetailsViewModel)
        {
            if (signInManager.IsSignedIn(User))
            {
                var domainModel = new BlogPostComment
                {
                    BlogPostId = blogDetailsViewModel.Id,
                    Description = blogDetailsViewModel.CommentDescription,
                    UserId = Guid.Parse(userManager.GetUserId(User)),
                    DateAdded=DateTime.Now,
                };
                await blogPostCommentRepository.AddAsync(domainModel);
                return RedirectToAction("Index", "Blogs", new {urlHandle=blogDetailsViewModel.UrlHandle});
            }
            return View();
        }

        //Get Comment by Id
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var comment = await blogPostCommentRepository.GetCommentByIdAsync(id);

            if (comment!= null) 
            {
                var getComment = new EditComment
                {
                    //Id = comment.UserId, //accepting user id
                    CommentId=comment.Id,
                    BlogPostId = comment.BlogPostId,
                    Description = comment.Description,
                    DateAdded = comment.DateAdded,
                };
                return View(getComment);
    }
            return View();
}

        //Update or Edit comment
        [HttpPost]
        public async Task<IActionResult> Edit(EditComment blogPostComment)
        {
            var comment = new BlogPostComment
            {
                Id=blogPostComment.Id,//comment id
                BlogPostId = blogPostComment.BlogPostId,
                Description = blogPostComment.Description,
                DateAdded = blogPostComment.DateAdded,
            };
            var updateComment = await blogPostCommentRepository.UpdateCommentAsync(comment); // go and search then return 
            if (updateComment != null)
            {
                //Show Success notification
                return RedirectToAction("Edit", new { id = blogPostComment.Id });
            }
            else
            {
                //Show error notification
            }
            //Show failure notification
            return RedirectToAction("Edit", new { id = blogPostComment.Id });
        }

        //Delete comment by id
        public async Task<IActionResult> Delete(EditComment editComment)
        {
            var deletedComment = await blogPostCommentRepository.DeleteAsync(editComment.Id);

            if (deletedComment != null)
            {
                return RedirectToAction("Edit");
            }
            //Show failed message
            return RedirectToAction("Edit", new { id = editComment.Id });
        }
    }
}
