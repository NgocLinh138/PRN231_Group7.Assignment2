namespace PRN231_Group7.Assignment2.Contract.Service.BookAuthor
{
    public class Request
    {
        public record AddBookAuthor(
            Guid Id,
            string AuthorOrder,
            string RoyaltyPercentage
        );
    }
}
