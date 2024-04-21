using PublisherResponse = PRN231_Group7.Assignment2.Contract.Service.Publisher.Response;


namespace PRN231_Group7.Assignment2.Contract.Service.Book
{
    public class Response
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public double Price { get; set; }
        public double? Advance { get; set; }
        public string? Royalty { get; set; }
        public string? YtdSales { get; set; }
        public string? Notes { get; set; }
        public DateTime PublishedDate { get; set; }
        public PublisherResponse Publisher { get; set; }
    }
}
