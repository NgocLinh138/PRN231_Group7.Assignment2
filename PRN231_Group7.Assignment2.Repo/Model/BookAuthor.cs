using System.ComponentModel.DataAnnotations.Schema;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace PRN231_Group7.Assignment2.Repo.Model
{
    public class BookAuthor
    {
        [Column("BookAuthorId")]
        public Guid Id { get; set; }
        public string AuthorOrder { get; set; }
        public string RoyaltyPercentage { get; set; }

        [ForeignKey(nameof(Book))]
        public Guid BookId { get; set; }

        [ForeignKey(nameof(Author))]
        public Guid AuthorId { get; set; }

        public Book Book { get; set; }
        public Author Author { get; set; }
    }
}
