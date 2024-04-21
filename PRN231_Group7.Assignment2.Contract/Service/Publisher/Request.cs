using System.ComponentModel.DataAnnotations;

namespace PRN231_Group7.Assignment2.Contract.Service.Publisher
{
    public class Request
    {
        public record AddPublisher (
            [Required(ErrorMessage = "{0} is required")]
            [MinLength(3, ErrorMessage = "{0} must have a minimum of 3 characters")]
            string PublisherName,
            string? City,
            string? State,
            string? Country
        );

        public record UpdatePublisher(
            [Required(ErrorMessage = "{0} is required")]
            [MinLength(3, ErrorMessage = "{0} must have a minimum of 3 characters")]
            string PublisherName,
            string? City,
            string? State,
            string? Country
        );
    }
}
