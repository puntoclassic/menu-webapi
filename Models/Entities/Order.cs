using System.Collections;
using System.ComponentModel.DataAnnotations.Schema;
using MenuBackend.Models.Auth;

namespace MenuBackend.Models.Entities
{

    public class Order
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }
        public int? OrderStateId { get; set; }
        public OrderState? OrderState { get; set; }
        public bool IsPaid { get; set; } = false;
        public bool IsShippingRequired { get; set; } = false;
        public string? DeliveryAddress { get; set; }
        public string? DeliveryTime { get; set; }
        public string? Notes { get; set; }
        public float ShippingCosts { get; set; } = 2;

        public ICollection<OrderDetail>? OrderDetails { get; set; }

        public float Total { get; set; }



    }
}
