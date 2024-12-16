using System.ComponentModel.DataAnnotations;

namespace Blogspot.Models.ViewModel
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage ="Username is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage ="Password must has atleast 6 characters")]
        public string Password { get; set; }
    }
}
