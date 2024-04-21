using System.ComponentModel.DataAnnotations;

namespace PRN231_Group7.Assignment2.Contract.Service.Role
{
    public class Request
    {
        public record AddRole(
            [Required(ErrorMessage = "{0} is required")]
            [MinLength(2, ErrorMessage = "{0} must have a minimum of {1} characters")]
            string RoleName
        );

        public record UpdateRole(
            [Required(ErrorMessage = "{0} is required")]
            [MinLength(2, ErrorMessage = "{0} must have a minimum of {1} characters")]
            string RoleName
        );

    }
}
