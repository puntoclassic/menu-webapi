using System.ComponentModel.DataAnnotations;
namespace MenuWebapi.Models.InputModel
{
    public class PostDeleteCategoryRequest
    {
        [Required]
        public int Id { get; set; }
    }
}
