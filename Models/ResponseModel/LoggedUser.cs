using System.Text.Json.Serialization;
namespace MenuWebapi.Models.ResponseModel
{
    public class LoggedUser
    {
        public bool? Verified { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; } = "user";
    }
}
