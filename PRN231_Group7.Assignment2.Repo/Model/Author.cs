using System.ComponentModel.DataAnnotations.Schema;

namespace PRN231_Group7.Assignment2.Repo.Model
{
    public class Author
    {
        [Column("AuthorId")]
        public Guid Id { get; set; }
        public string EmailAddress { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Zip { get; set; }

        public ICollection<BookAuthor> BookAuthor { get; set; }
    }
}
