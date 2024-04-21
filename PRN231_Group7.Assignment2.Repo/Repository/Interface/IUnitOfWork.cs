namespace PRN231_Group7.Assignment2.Repo.Repository.Interface
{
    public interface IUnitOfWork
    {
        BookRepository BookRepository { get; }
        RoleRepository RoleRepository { get; }
        PublisherRepository PublisherRepository { get; }
        AuthorRepository AuthorRepository { get; }
        UserRepository UserRepository { get; }
        void Save();



    }
}
