using System.ComponentModel.DataAnnotations;
namespace MenuBackend.Models.InputModel
{
    public class PostDeleteOrderState
    {
        [Required]
        public int Id { get; set; }
    }
}
