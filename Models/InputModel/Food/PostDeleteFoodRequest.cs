using System.ComponentModel.DataAnnotations;
namespace MenuWebapi.Models.InputModel
{
    public class PostDeleteFoodRequest
    {
        [Required]
        public int Id { get; set; }
    }
}
