using System.ComponentModel.DataAnnotations;
namespace MenuWebapi.Models.InputModel
{
    public class PostUpdateOrderState
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        public string? CssBadgeClass { get; set; }
    }
}
