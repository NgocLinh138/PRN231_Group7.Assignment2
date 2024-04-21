using Microsoft.EntityFrameworkCore;
using PRN231_Group7.Assignment2.Repo.DataAccess;
using PRN231_Group7.Assignment2.Repo.Model;

namespace PRN231_Group7.Assignment2.Repo.Repository
{
    public class UserRepository : GenericRepository<User>
    {
        private readonly BookDbContext context;
        public UserRepository(BookDbContext context) : base(context)
        {
            this.context = context;
        }

        public bool IsExistUser(Guid id) => context.Users.Find(id) == null ? false : true;

        public async Task<User> FindByEmailAsync(string email)
            => await context.Users.FirstOrDefaultAsync(x => x.EmailAddress == email);
    }
}
