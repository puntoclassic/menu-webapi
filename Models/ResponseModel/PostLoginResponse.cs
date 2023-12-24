using System.Runtime.Serialization;
using System.Text.Json.Serialization;
namespace MenuWebapi.Models.ResponseModel
{
    public class PostLoginResponse
    {
        public string? Status { get; set; }
        public LoggedUser? User { get; set; }
    }
}
