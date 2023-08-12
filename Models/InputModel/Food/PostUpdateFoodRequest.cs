using System.ComponentModel.DataAnnotations;
using MenuBackend.Models.Annotations;
namespace MenuBackend.Models.InputModel
{
    public class PostUpdateFoodRequest
    {
        [Required]
        public int Id { get; set; }
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
