namespace PRN231_Group7.Assignment2.Contract.Service.BookAuthor
{
    public class Response
    {
        public record BookAuthorResponse(
            Guid Id,
            string AuthorOrder,
            string RoyaltyPercentage,
            Guid BookId,
            string BookTitle,
            string BookType,
            Guid AuthorId,
            string AuthorName
        );
    }
}
