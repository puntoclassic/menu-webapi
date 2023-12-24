using System.ComponentModel.DataAnnotations;
using MenuWebapi.Models.Annotations;
namespace MenuWebapi.Models.InputModel
{
    public class PostCreateFoodRequest
    {
        [Required]
        public string? Name { get; set; }
        public string? Ingredients { get; set; }
        [Required]
        [Range(0.01, float.MaxValue)]
        public float Price { get; set; }
        [Required]
        [CategoryExists]
        public int CategoryId { get; set; }
    }
}
