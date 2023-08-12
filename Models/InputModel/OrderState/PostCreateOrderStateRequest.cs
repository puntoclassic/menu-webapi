using System.ComponentModel.DataAnnotations;
namespace MenuBackend.Models.InputModel
{
    public class PostCreateOrderState
    {
        [Required]
        public string? Name { get; set; }
        public string? CssBadgeClass { get; set; }
    }
}
