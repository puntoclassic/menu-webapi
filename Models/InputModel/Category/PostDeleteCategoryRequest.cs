using System.ComponentModel.DataAnnotations;
namespace MenuBackend.Models.InputModel
{
    public class PostDeleteCategoryRequest
    {
        [Required]
        public int Id { get; set; }
    }
}
