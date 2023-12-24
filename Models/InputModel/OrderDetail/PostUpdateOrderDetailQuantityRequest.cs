using System.ComponentModel.DataAnnotations;

namespace MenuWebapi.Models.InputModel
{

    public class PostUpdateOrderDetailQuantityRequest
    {
        [Required]
        public int OrderId { get; set; }
        [Required]
        public int OrderDetailId { get; set; }
        [Required]
        public int Quantity { get; set; }
    }
}
