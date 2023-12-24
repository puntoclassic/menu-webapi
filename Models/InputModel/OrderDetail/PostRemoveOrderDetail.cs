using System.ComponentModel.DataAnnotations;

namespace MenuWebapi.Models.InputModel
{

    public class PostRemoveOrderDetail
    {
        [Required]
        public int OrderId { get; set; }
        [Required]
        public int OrderDetailId { get; set; }

    }
}
