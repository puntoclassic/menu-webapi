using MenuWebapi.Models.Entities;
namespace MenuWebapi.Models.ResponseModel;
public class GetSearchFoodResponse
{
    public int Count { get; set; } = 0;
    public List<Food> Items { get; set; } = new List<Food>();
}
