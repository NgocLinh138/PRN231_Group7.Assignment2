using System.ComponentModel.DataAnnotations;

namespace PRN231_Group7.Assignment2.UI.Models.Publisher
{
    public class PublisherRequestModel
    {
        public record CreatePublisher (
            [Required(ErrorMessage = "{0} is required")]
            [MinLength(3, ErrorMessage = "{0} must have a minimum of 3 characters")]
            string PublisherName,
            string? City,
            string? State,
            string? Country
        );

        public record UpdatePublisher(
            Guid Id,
            [Required(ErrorMessage = "{0} is required")]
            [MinLength(3, ErrorMessage = "{0} must have a minimum of 3 characters")]
            string PublisherName,
            string? City,
            string? State,
            string? Country
        );

    }
}
