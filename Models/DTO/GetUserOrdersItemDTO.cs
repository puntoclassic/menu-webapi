
namespace MenuBackend.Models.DTO;


public class GetUserOrdersItemDTO
{
    public int Id { get; set; }

    public OrderStateDTO? OrderState { get; set; }

    public float Total { get; set; }


}

