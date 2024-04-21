using System.ComponentModel.DataAnnotations;

namespace PRN231_Group7.Assignment2.UI.Models.User
{
    public class UserRequestModel
    {
        public record UpdateUser
        (
            Guid Id,

            [Required(ErrorMessage = "{0} is required")]
            [EmailAddress(ErrorMessage = "{0} should be a proper email.")]
            string EmailAddress,

            [Required(ErrorMessage = "{0} is required")]
            string FirstName,

            [Required(ErrorMessage = "{0} is required")]
            string LastName,

            [Required(ErrorMessage = "{0} is required")]
            [Phone(ErrorMessage = "{0} should contain 10 digits.")]
            string Phone,

            [Required(ErrorMessage = "{0} is required")]
            string Password,

            string? Source,

            [Required(ErrorMessage = "{0} is required")]
            DateTime HireDate

            //[Required(ErrorMessage = "{0} is required")]
            //Guid RoleId
        );







        
         


        
    }
}
