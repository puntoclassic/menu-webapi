using MenuBackend.Models.Entities;

namespace MenuBackend.Models.DTO
{

    public class OrderGetAllDTO
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public UserDTO? User { get; set; }
        public int? OrderStateId { get; set; }
        public OrderStateDTO? OrderState { get; set; }

        public float Total { get; set; }


    }
}
