using System.ComponentModel.DataAnnotations;
using MenuWebapi.Models.Annotations;
namespace MenuWebapi.Models.InputModel
{
    public class PostCreateAccountRequest
    {
        [Required]
        public string? FirstName { get; set; }
        [Required]
        public string? LastName { get; set; }
        [Required]
        [EmailAddress]
        [EmailUnique]
        public string? Email { get; set; }
        [Required]
        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$")]
        public string? Password { get; set; }
        [Required]
        [Compare("Password")]
        public string? ConfirmPassword { get; set; }
    }
}
