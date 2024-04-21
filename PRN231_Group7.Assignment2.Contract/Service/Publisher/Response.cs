namespace PRN231_Group7.Assignment2.Contract.Service.Publisher
{
    public class Response
    {
        public Guid Id { get; set; }
        public string PublisherName { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
    }
}
