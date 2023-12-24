using Microsoft.AspNetCore.Identity;
namespace MenuWebapi.Models.Auth
{
    public class ApplicationUser : IdentityUser
    {
        public string Firstname { get; set; } = "";
        public string Lastname { get; set; } = "";
    }
}
