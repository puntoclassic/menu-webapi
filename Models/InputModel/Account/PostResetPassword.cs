using System.ComponentModel.DataAnnotations;
namespace MenuWebapi.Models.InputModel
{
    public class PostResetPassword
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
    }
}
