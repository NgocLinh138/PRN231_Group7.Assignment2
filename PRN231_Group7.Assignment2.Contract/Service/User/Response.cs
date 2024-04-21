using PublisherResponse = PRN231_Group7.Assignment2.Contract.Service.Publisher.Response;
using RoleResponse = PRN231_Group7.Assignment2.Contract.Service.Role.Response;

namespace PRN231_Group7.Assignment2.Contract.Service.User
{
    public class Response
    {
        public Guid Id { get; set; }
        public string EmailAddress { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string? Source { get; set; }
        public DateTime HireDate { get; set; }
        public PublisherResponse Publisher {  get; set; }
        public RoleResponse Role { get; set; }

    }
}
