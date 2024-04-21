namespace PRN231_Group7.Assignment2.Repo.Services.Options
{
    public class JWTOption
    {
        public string Issuer { get; set; }

        public string Audience { get; set; }

        public string SecretKey { get; set; }

        public int ExpireMin { get; set; }
    }
}
