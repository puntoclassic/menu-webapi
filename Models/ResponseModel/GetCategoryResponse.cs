using MenuWebapi.Models.Entities;
namespace MenuWebapi.Models.ResponseModel;
public class GetCategoryResponse
{
    public int Count { get; set; } = 0;
    public List<Category> Items { get; set; } = new List<Category>();
}
