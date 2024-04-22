using System.ComponentModel.DataAnnotations;

namespace PRN231_Group7.Assignment2.Contract.Service.Book
{
    public class Request
    {
        public record AddBook(
            [Required(ErrorMessage = "{0} is required")]
            [MinLength(3, ErrorMessage = "{0} must have a minimum of {1} characters")]
            string Title,

            [Required(ErrorMessage = "{0} is required")]
            string Type,

            [Required(ErrorMessage = "{0} is required")]
            Guid PublisherId,

            [Required(ErrorMessage = "{0} is required")]
            [Range(0, double.MaxValue, ErrorMessage = "{0} must be greater than or equal to {1}")]
            double Price,

            double? Advance,
            string? Royalty,
            string? YtdSales,
            string? Notes,

            [Required(ErrorMessage = "{0} is required")]
            DateTime PublishedDate
        );


        public record UpdateBook(
            [Required(ErrorMessage = "{0} is required")]
            [MinLength(3, ErrorMessage = "{0} must have a minimum of {1} characters")]
            string Title,

            [Required(ErrorMessage = "{0} is required")]
            string Type,

            //[Required(ErrorMessage = "{0} is required")]
            //Guid PublisherId,

            [Required(ErrorMessage = "{0} is required")]
            [Range(0, double.MaxValue, ErrorMessage = "{0} must be greater than or equal to {1}")]
            double Price,

            double? Advance,
            string? Royalty,
            string? YtdSales,
            string? Notes,

            [Required(ErrorMessage = "{0} is required")]
            DateTime PublishedDate,
             Guid PublisherId
        );

    }
}
