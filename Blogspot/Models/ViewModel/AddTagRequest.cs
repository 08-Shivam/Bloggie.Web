using System.ComponentModel.DataAnnotations;

namespace Blogspot.Models.ViewModel
{
    public class AddTagRequest
    {

        [Required(ErrorMessage ="Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage ="Display Name is required")]
        public string DisplayName { get; set; }

    }
}
