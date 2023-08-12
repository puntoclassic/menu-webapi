using System.ComponentModel.DataAnnotations;
namespace MenuBackend.Models.InputModel
{
    public class PostResetPassword
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
    }
}
