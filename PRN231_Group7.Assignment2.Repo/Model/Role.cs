using System.ComponentModel.DataAnnotations.Schema;

namespace PRN231_Group7.Assignment2.Repo.Model
{
    public class Role
    {
        [Column("RoleId")]
        public Guid Id { get; set; }
        public string RoleName { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
