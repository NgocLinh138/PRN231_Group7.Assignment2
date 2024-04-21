using PRN231_Group7.Assignment2.Repo.DataAccess;
using PRN231_Group7.Assignment2.Repo.Repository.Interface;

namespace PRN231_Group7.Assignment2.Repo.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private BookDbContext context = new BookDbContext();

        public UnitOfWork(BookDbContext context)
        {
            this.context = context;
        }

        private BookRepository bookRepository;
        private RoleRepository roleRepository;
        private PublisherRepository publisherRepository;
        private AuthorRepository authorRepository;
        private UserRepository userRepository;

        public BookRepository BookRepository
        {
            get
            {
                if (bookRepository == null)
                {
                    bookRepository = new BookRepository(context);
                }
                return bookRepository;
            }
        }

        public RoleRepository RoleRepository
        {
            get
            {
                if (roleRepository == null)
                {
                    roleRepository = new RoleRepository(context);
                }
                return roleRepository;
            }
        }


        public PublisherRepository PublisherRepository
        {
            get
            {
                if (publisherRepository == null)
                {
                    publisherRepository = new PublisherRepository(context);
                }
                return publisherRepository;
            }
        }

        public AuthorRepository AuthorRepository
        {
            get
            {
                if(authorRepository == null)
                {
                    authorRepository = new AuthorRepository(context);
                }
                return authorRepository;
            }
        }

        public UserRepository UserRepository
        {
            get
            {
                if(userRepository == null)
                {
                    userRepository = new UserRepository(context);
                }
                return userRepository;
            }
        }

     

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}
