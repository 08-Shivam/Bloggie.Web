using System.ComponentModel.DataAnnotations;

namespace Blogspot.Models.ViewModel
{
    public class LoginViewModel
    {
        [Required(ErrorMessage ="Username is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(1, ErrorMessage ="Password must has atleast 6 characters")]
        public string Password { get; set; }

        public string? ReturnUrl {  get; set; }
    }
}
