using System.ComponentModel.DataAnnotations;

namespace PRN231_Group7.Assignment2.Contract.Service.Author
{
    public class Request
    {
        public record AddAuthor(
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
