using MenuBackend.Models.Entities;
namespace MenuBackend.Models.ResponseModel;
public class GetFoodResponse
{
    public int Count { get; set; } = 0;
    public List<Food> Items { get; set; } = new List<Food>();
}
