namespace MenuWebapi.Models.Options
{
    public class JwtOptions
    {
        public string? ValidIssuer { get; set; }
        public string? ValidAudience { get; set; }
        public string? Key { get; set; }
    }
}
