using System.ComponentModel.DataAnnotations;
namespace MenuWebapi.Models.InputModel
{
    public class PostCreateCategoryRequest
    {
        [Required]
        public string? Name { get; set; }
        public IFormFile? Image { get; set; }
    }
}
