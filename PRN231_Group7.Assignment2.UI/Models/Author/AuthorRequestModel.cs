using System.ComponentModel.DataAnnotations;

namespace PRN231_Group7.Assignment2.UI.Models.Author
{
    public class AuthorRequestModel
    {
        public record CreateAuthor(
          [Required(ErrorMessage = "{0} is required")]
          [EmailAddress(ErrorMessage = "{0} should be a proper email.")]
          string EmailAddress,

          [Required(ErrorMessage = "{0} is required")]
          string FirstName,

          [Required(ErrorMessage = "{0} is required")]
          string LastName,

          [Required(ErrorMessage = "{0} is required")]
          [Phone(ErrorMessage = "{0} should contain 10 digits.")]
          string Phone
        );


        public record UpdateAuthor(
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

          string? Address,
          string? City,
          string? State,
          string? Zip
        );

    }
}
