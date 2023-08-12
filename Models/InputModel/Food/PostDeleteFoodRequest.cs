using System.ComponentModel.DataAnnotations;
namespace MenuBackend.Models.InputModel
{
    public class PostDeleteFoodRequest
    {
        [Required]
        public int Id { get; set; }
    }
}
