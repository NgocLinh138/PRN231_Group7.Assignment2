using PRN231_Group7.Assignment2.Repo.DataAccess;
using PRN231_Group7.Assignment2.Repo.Model;

namespace PRN231_Group7.Assignment2.Repo.Repository
{
    public class RoleRepository : GenericRepository<Role>
    {
        private readonly BookDbContext context;
        public RoleRepository(BookDbContext context) : base(context)
        {
            this.context = context;
        }

        public bool IsRoleExist(Guid id) => context.Roles.Find(id) == null ? false : true;

        public Role GetCustomerRole()
            => context.Roles.FirstOrDefault(x => x.RoleName.ToLower().Contains("customer"));
    }
}
