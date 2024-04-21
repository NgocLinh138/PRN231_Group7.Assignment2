using System.ComponentModel.DataAnnotations.Schema;

namespace PRN231_Group7.Assignment2.Repo.Model
{
    public class Publisher
    {
        [Column("PublisherId")]
        public Guid Id { get; set; }
        public string PublisherName { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }

        public ICollection<Book> Books { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
