using Microsoft.EntityFrameworkCore;
using PRN231_Group7.Assignment2.Repo.Model;

namespace PRN231_Group7.Assignment2.Repo.DataAccess
{
    public class BookDbContext : DbContext
    {

        public BookDbContext()
        {
        }

        public BookDbContext(DbContextOptions<BookDbContext> options) : base(options)
        {
        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<BookAuthor> BookAuthors { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("ConnectionStrings:DefaultConnection");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Author>().HasData(
                new Author { Id = Guid.NewGuid(), EmailAddress = "john@gmail.com", FirstName = "John", LastName = "Doe", Phone = "0123456789", Address = "123 Main St", City = "HCM", State = "State", Zip = "12345"},
                new Author { Id = Guid.NewGuid(), EmailAddress = "alice@gmail.com", FirstName = "Alice", LastName = "Smith", Phone = "0123123123", Address = "456 Elm St", City = "Vung Tau", State = "State", Zip = "54321"}
            );

            modelBuilder.Entity<Publisher>().HasData(
                new Publisher { Id = Guid.NewGuid(), PublisherName = "Publisher A", City = "City A", State = "State A", Country = "Country A" },
                new Publisher { Id = Guid.NewGuid(), PublisherName = "Publisher B", City = "City B", State = "State B", Country = "Country B" }
            );


        }


    }
}
