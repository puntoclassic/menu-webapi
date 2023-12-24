using System.ComponentModel.DataAnnotations;
namespace MenuWebapi.Models.InputModel
{
    public class PostChangePasswordByTokenRequest
    {
        [Required]
        public string? Token { get; set; }
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$")]
        public string? Password { get; set; }
        [Required]
        [Compare("Password")]
        public string? ConfirmPassword { get; set; }
    }
}
