using System.ComponentModel.DataAnnotations;
namespace MenuWebapi.Models.InputModel
{
    public class PostCreateOrderState
    {
        [Required]
        public string? Name { get; set; }
        public string? CssBadgeClass { get; set; }
    }
}
