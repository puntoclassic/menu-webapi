using MenuWebapi.Models.Entities;

namespace MenuWebapi.Models.DTO
{

    public class OrderDTO
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public UserDTO? User { get; set; }
        public int? OrderStateId { get; set; }
        public OrderStateDTO? OrderState { get; set; }
        public bool IsPaid { get; set; } = false;
        public bool IsShippingRequired { get; set; } = false;
        public string? DeliveryAddress { get; set; }
        public string? DeliveryTime { get; set; }
        public string? Notes { get; set; }
        public float ShippingCosts { get; set; } = 2;

        public ICollection<OrderDetailDTO>? OrderDetails { get; set; }

        public float Total { get; set; }


    }
}
