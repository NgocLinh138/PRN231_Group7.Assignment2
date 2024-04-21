using PRN231_Group7.Assignment2.Repo.DataAccess;
using PRN231_Group7.Assignment2.Repo.Model;

namespace PRN231_Group7.Assignment2.Repo.Repository
{
    public class AuthorRepository : GenericRepository<Author>
    {
        public AuthorRepository(BookDbContext context) : base(context)
        {
        }

        public bool IsAuthorExist(Guid id) => context.Authors.Find(id) == null ? false : true;
    }
}
