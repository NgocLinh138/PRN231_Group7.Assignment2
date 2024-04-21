using PRN231_Group7.Assignment2.Repo.DataAccess;
using PRN231_Group7.Assignment2.Repo.Model;

namespace PRN231_Group7.Assignment2.Repo.Repository
{
    public class PublisherRepository : GenericRepository<Publisher>
    {
        public PublisherRepository(BookDbContext context) : base(context)
        {
        }

        public bool IsExistPublisher(Guid id) => context.Publishers.Find(id) == null ? false : true;

    }
}
