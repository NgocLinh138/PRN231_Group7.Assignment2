namespace PRN231_Group7.Assignment2.Contract.Service.Author
{
    public class Response
    {
        public record AuthorRepsonse(
          Guid Id,
          string EmailAddress,
          string FirstName,
          string LastName,
          string Phone,
          string? Address,
          string? City,
          string? State,
          string? Zip
        );



    }
}
