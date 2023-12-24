using System.ComponentModel.DataAnnotations;
namespace MenuWebapi.Models.InputModel
{
    public class PostRequireNewTokenRequest
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
    }
}
