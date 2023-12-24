using System.ComponentModel.DataAnnotations;
namespace MenuWebapi.Models.InputModel
{
    public class PostLoginRequest
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}
