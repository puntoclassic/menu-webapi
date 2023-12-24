using System.ComponentModel.DataAnnotations;
namespace MenuWebapi.Models.InputModel
{
    public class PostDeleteOrderState
    {
        [Required]
        public int Id { get; set; }
    }
}
