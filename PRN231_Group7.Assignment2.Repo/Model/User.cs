using System.ComponentModel.DataAnnotations.Schema;

namespace PRN231_Group7.Assignment2.Repo.Model
{
    public class User
    {
        [Column("UserId")]
        public Guid Id { get; set; }

        public string EmailAddress { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public string? Source { get; set; }
        public DateTime HireDate { get; set; }

        [ForeignKey("Publisher")]
        public Guid PubId { get; set; }


        [ForeignKey("Role")]
        public Guid RoleId { get; set; }

        public Role Role { get; set; }
        public Publisher Publisher { get; set; }
    }
}
