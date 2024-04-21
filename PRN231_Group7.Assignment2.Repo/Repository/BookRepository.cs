using PRN231_Group7.Assignment2.Repo.DataAccess;
using PRN231_Group7.Assignment2.Repo.Model;

namespace PRN231_Group7.Assignment2.Repo.Repository
{
    public class BookRepository : GenericRepository<Book>
    {
        public BookRepository(BookDbContext context) : base(context)
        {
        }

        public bool IsExistBook(Guid id) => context.Books.Find(id) == null ? false : true;   

    }
}
