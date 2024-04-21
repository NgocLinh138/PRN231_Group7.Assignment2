namespace PRN231_Group7.Assignment2.UI.Models.Authentication
{
    public class RegisterModel
    {
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Source { get; set; }
        public DateTime HireDate { get; set; }
        public Guid PublisherId { get; set; }
    }
}
