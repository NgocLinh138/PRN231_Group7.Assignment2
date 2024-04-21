using System.ComponentModel.DataAnnotations;

namespace PRN231_Group7.Assignment2.UI.Models.Authentication
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "{0} should be a proper email.")]
        public string EmailAddress;


        [Required(ErrorMessage = "Password is required")]
        public string Password;
    }
}
