using Blogspot.Models.ViewModel;
using Blogspot.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Blogspot.Models.Domain;
using Microsoft.AspNetCore.Authorization;

namespace Blogspot.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminBlogPostsController : Controller
    {
        private readonly ITagRepository tagRepository;
        private readonly IBlogPostRepository blogPostRepository;
        public AdminBlogPostsController(ITagRepository tagRepository, IBlogPostRepository blogPostRepository)
        {
            this.tagRepository = tagRepository;
            this.blogPostRepository = blogPostRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var tags = await tagRepository.GetAllAsync();

            var model = new AddBlogPostRequest
            {
                Tags = tags.Select(x=>new SelectListItem { Text=x.Name, Value=x.Id.ToString()})
            };
            //Get All tag
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddBlogPostRequest addBlogPostRequest) 
        {
            //Map view model to domain model
            var blogPost = new BlogPost
            {
                Heading=addBlogPostRequest.Heading,
                PageTitle=addBlogPostRequest.PageTitle,
                Content=addBlogPostRequest.Content,
                ShortDescription=addBlogPostRequest.ShortDescription,
                FeaturedImageUrl=addBlogPostRequest.FeaturedImageUrl,
                UrlHandle=addBlogPostRequest.UrlHandle,
                PublishedDate=addBlogPostRequest.PublishedDate,
                Author=addBlogPostRequest.Author,
                Visible=addBlogPostRequest.Visible,
            };

            //Map tag from selected tag
            var selectedTags=new List<Tag>(); //this will be get filled once tags found using id
            foreach (var selectedTagId in addBlogPostRequest.SelectedTags) 
            {
                var selectedTagIdAsGuid=Guid.Parse(selectedTagId); //search for the id through the database
                var existingTag=await tagRepository.GetAsync(selectedTagIdAsGuid);

                if (existingTag != null) 
                {
                    selectedTags.Add(existingTag);
                }
                await tagRepository.GetAsync(selectedTagIdAsGuid); //get tag by id
            }
            //Mapping tags back to domain model
            blogPost.Tags = selectedTags;

            await blogPostRepository.AddAsync(blogPost);
            return RedirectToAction("Add");
        }

        //View all Blogs
        [HttpGet]
        [ActionName("List")]
        public async Task<IActionResult> List(string? searchQuery,
            string? sortBy, string? sortDirection, int pageSize = 3, int pageNumber = 1)
        {
            var totalRecords = await blogPostRepository.CountAsync();
            var totalPages = Math.Ceiling((decimal)totalRecords / pageSize);

            if (pageNumber > totalPages)
            {
                pageNumber--;
            }
            if (pageNumber < 1)
            {
                pageNumber++;
            }

            ViewBag.TotalPages = totalPages;
            ViewBag.PageSize = pageSize;
            ViewBag.SearchQuery = searchQuery;
            ViewBag.SortBy = sortBy;
            ViewBag.SortDirection = sortDirection;
            ViewBag.PageNumber = pageNumber;

            //Call the repository
            var blogPosts=await blogPostRepository.GetAllAsync(searchQuery, sortBy, sortDirection, pageNumber, pageSize);
            return View(blogPosts);
        }

        //Edit Blogs
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id) 
        {
            //Retrieve the result from the repository
            var blogPost=await blogPostRepository.GetAsync(id); //for blogpost
            var tagDomainModel = await tagRepository.GetAllAsync();//for tags

            //pass the data to view
            if (blogPost!=null)
            {
                var model = new EditBlogPostRequest
                {
                    Id = blogPost.Id,
                    Heading = blogPost.Heading,
                    PageTitle = blogPost.PageTitle,
                    Content = blogPost.Content,
                    Author = blogPost.Author,
                    FeaturedImageUrl = blogPost.FeaturedImageUrl,
                    UrlHandle = blogPost.UrlHandle,
                    ShortDescription = blogPost.ShortDescription,
                    PublishedDate = blogPost.PublishedDate,
                    Visible = blogPost.Visible,

                    Tags = tagDomainModel.Select(x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Id.ToString(),
                    }),
                    SelectedTags = blogPost.Tags.Select(x => x.Id.ToString()).ToArray()
                };
                //pass the data to view
                return View(model);
            }
            
            return View(null);
        }

        //Update Blogs
        [HttpPost]
        public async Task<IActionResult> Edit(EditBlogPostRequest editBlogPostRequest)
        {
            //map view model to domain model
            var blogPostDomainModel = new BlogPost
            {
                Id=editBlogPostRequest.Id,
                Heading = editBlogPostRequest.Heading,
                PageTitle = editBlogPostRequest.PageTitle,
                Content = editBlogPostRequest.Content,
                Author = editBlogPostRequest.Author,
                ShortDescription=editBlogPostRequest.ShortDescription,
                FeaturedImageUrl=editBlogPostRequest.FeaturedImageUrl,
                PublishedDate=editBlogPostRequest.PublishedDate,
                UrlHandle=editBlogPostRequest.UrlHandle,
                Visible = editBlogPostRequest.Visible,
            };
            //map tags to domain model
            var selectedTags = new List<Tag>();
            foreach (var selectedTag in editBlogPostRequest.SelectedTags)
            {
                if (Guid.TryParse(selectedTag, out var tag))
                {
                    var foundTag = await tagRepository.GetAsync(tag);
                    if (foundTag != null)
                    {
                        selectedTags.Add(foundTag);
                    }
                }
            }
            blogPostDomainModel.Tags=selectedTags;

            //submit information to repository to update
            var updatedBlog=await blogPostRepository.UpdateAsync(blogPostDomainModel);
            if(updatedBlog != null)
            {
                return RedirectToAction("Edit");
            }

            //redirect to get method
            return RedirectToAction("Edit");
        }

        //Delete Blogs
        [HttpPost]
        public async Task<IActionResult> Delete(EditBlogPostRequest editBlogPostRequest)
        {
            //Talk to repository to delete 
            var deletedBlogPost= await blogPostRepository.DeleteAsync(editBlogPostRequest.Id);

            if(deletedBlogPost != null)
            {
                return RedirectToAction("List");
            }
            //Display the response
            return RedirectToAction("Edit", new {id=editBlogPostRequest.Id });
        }
    }
}
