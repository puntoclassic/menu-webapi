using System.ComponentModel.DataAnnotations;
namespace MenuBackend.Models.InputModel
{
    public class PostUpdateCategoryRequest
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        public IFormFile? Image { get; set; }
    }
}
