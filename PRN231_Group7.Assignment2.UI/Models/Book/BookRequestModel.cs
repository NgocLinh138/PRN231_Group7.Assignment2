using System.ComponentModel.DataAnnotations;

namespace PRN231_Group7.Assignment2.UI.Models.Book
{
    public class BookRequestModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [MinLength(3, ErrorMessage = "{0} must have a minimum of {1} characters")]
        public string Title { get; set; }


        [Required(ErrorMessage = "{0} is required")]
        public string Type { get; set; }


        [Required(ErrorMessage = "{0} is required")]
        [Range(0, double.MaxValue, ErrorMessage = "{0} must be greater than or equal to {1}")]
        public double Price { get; set; }

        public double? Advance { get; set; }
        public string? Royalty { get; set; }
        public string? YtdSales { get; set; }
        public string? Notes { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        public DateTime PublishedDate { get; set; }


        [Required(ErrorMessage = "{0} is required")]
        public Guid PublisherId { get; set; }


    }
}
