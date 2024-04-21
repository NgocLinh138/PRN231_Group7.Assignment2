using System.ComponentModel.DataAnnotations.Schema;

namespace PRN231_Group7.Assignment2.Repo.Model
{
    public class Book
    {

        [Column("BookId")]
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public double Price { get; set; }
        public double? Advance { get; set; }
        public string? Royalty { get; set; }
        public string? YtdSales { get; set; }
        public string? Notes { get; set; }
        public DateTime PublishedDate { get; set; }

        //public string BookImage { get; set; }


        public ICollection<BookAuthor> BookAuthors { get; set; }


        [ForeignKey("Publisher")]
        public Guid PublisherId { get; set; }
        public Publisher Publisher { get; set; }
    }
}
