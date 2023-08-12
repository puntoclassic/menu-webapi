using System.Runtime.Serialization;
using System.Text.Json.Serialization;
namespace MenuBackend.Models.ResponseModel
{
    public class PostLoginResponse
    {
        public string? Status { get; set; }
        public LoggedUser? User { get; set; }
    }
}
