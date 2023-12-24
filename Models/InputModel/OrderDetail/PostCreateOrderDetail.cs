using System.ComponentModel.DataAnnotations;

namespace MenuWebapi.Models.InputModel
{

    public class PostCreateOrderDetail
    {
        [Required]
        public int OrderId { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public float UnitPrice { get; set; }

    }
}
