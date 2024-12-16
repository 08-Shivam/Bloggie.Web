using Blogspot.Models.Domain;
using Blogspot.Models.ViewModel;
using Blogspot.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blogspot.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminTagController : Controller
    {
        private readonly ITagRepository tagRepository;
        public AdminTagController(ITagRepository tagRepository)
        {
            this.tagRepository = tagRepository;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        //Asynchronous method
        public async Task<IActionResult> Add(AddTagRequest addTagRequest)
        {
            ValidateAddTagRequest(addTagRequest);
            if (ModelState.IsValid==false)
            {
                return View();
            }
            var tag = new Tag
            {
                Name= addTagRequest.Name,
                DisplayName = addTagRequest.DisplayName,
            };
            await tagRepository.AddAsync(tag);

            return RedirectToAction("List");  //go to View Add.cshtml
        }

        //To View all tags in a list
        [HttpGet]
        [ActionName("List")]
        public async Task<IActionResult> List(string? searchQuery,
            string? sortBy, string? sortDirection, int pageSize=3,int pageNumber=1)
        {
            var totalRecords = await tagRepository.CountAsync();
            var totalPages = Math.Ceiling((decimal)totalRecords/pageSize);

            if (pageNumber > totalPages)
            {
                pageNumber--;
            }
            if(pageNumber<1)
            {
                pageNumber++;
            }

            ViewBag.TotalPages = totalPages;
            ViewBag.PageSize=pageSize;
            ViewBag.SearchQuery = searchQuery;
            ViewBag.SortBy = sortBy;
            ViewBag.SortDirection = sortDirection;
            ViewBag.PageNumber = pageNumber;    
            var tag= await tagRepository.GetAllAsync(searchQuery, sortBy, sortDirection, pageNumber,pageSize);
            return View(tag);
        }

        [HttpGet]
        //To Edit existing tag, first find the details based on id clicked on edit button on frontend of show all tags
        public async Task<IActionResult> Edit(Guid id)
        {
            var tag = await tagRepository.GetAsync(id); //fetch all details based on id 

            if (tag != null) {
                var editTagRequest = new EditTagRequest
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    DisplayName = tag.DisplayName,
                };
                return View(editTagRequest);// show the details fetched on frontend of edit.cshtml
            }
            return View(); 
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditTagRequest editTagRequest)
        {

            var tag = new Tag
            {
                Id = editTagRequest.Id,
                Name = editTagRequest.Name,
                DisplayName = editTagRequest.DisplayName,
            }; //received from edit.cshtml

            var updateTag=await tagRepository.UpdateAsync(tag);

            if (updateTag != null) {
                //Show Success notification
              
            }
            else
            {
                //Show error notification
            }
            //Show failure notification
            return RedirectToAction("Edit", new {id=editTagRequest.Id});
        }

        [HttpPost]
        public async Task<IActionResult> Delete(EditTagRequest editTagRequest)
        {
          
            var deletedTag= await tagRepository.DeleteAsync(editTagRequest.Id);

            if (deletedTag != null) 
            {
                //Show success notification
                return RedirectToAction("List");
            }
            //Show failed message
            return RedirectToAction("Edit", new {id=editTagRequest.Id});
        }

        //Validation method
        private void ValidateAddTagRequest(AddTagRequest request)
        {
            if (request.Name is not null && request.DisplayName is not null)
            {
                if (request.Name == request.DisplayName)
                {
                    ModelState.AddModelError("DisplayName", "Name cannot be same as DisplayName");
                }
            }
        }
    }
}
