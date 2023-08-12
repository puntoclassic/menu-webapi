using System.ComponentModel.DataAnnotations;
namespace MenuBackend.Models.InputModel
{
    public class PostRequireNewTokenRequest
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
    }
}
