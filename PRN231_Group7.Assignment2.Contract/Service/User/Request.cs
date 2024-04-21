using System.ComponentModel.DataAnnotations;

namespace PRN231_Group7.Assignment2.Contract.Service.User
{
    public class Request
    {
        public record AddUser(
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

          string Password,
          DateTime HireDate,
          Guid PubId,
          Guid RoleId
        );


        public record UpdateUser(
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

          string Password,
          string? Source,
          DateTime HireDate
          //Guid PubId,
          //Guid RoleId
        );

        public record RegisterUser(
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

          string Password,
          DateTime HireDate,
          Guid PublisherId,
          Guid RoleId
        );

        public record Login(
            [Required(ErrorMessage = "Email is required")]
            [EmailAddress(ErrorMessage ="{0} should be a proper email.")]
            string EmailAddress,

            [Required(ErrorMessage = "Password is required")]
            string Password
            );
    }
}
