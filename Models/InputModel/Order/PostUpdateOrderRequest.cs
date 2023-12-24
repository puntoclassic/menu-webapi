using System.ComponentModel.DataAnnotations;

namespace MenuWebapi.Models.InputModel
{

    public class PostUpdateOrderRequest
    {

        [Required]
        public int Id { get; set; }

        [Required]
        public int? OrderStateId { get; set; }

        [Required]
        public bool IsPaid { get; set; } = false;

        [Required]
        public bool IsShippingRequired { get; set; } = false;

        public string? DeliveryAddress { get; set; }
        public string? DeliveryTime { get; set; }
        public string? Notes { get; set; }
        public float ShippingCosts { get; set; } = 2;

    }
}
