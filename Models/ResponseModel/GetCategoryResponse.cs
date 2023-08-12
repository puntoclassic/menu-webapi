using MenuBackend.Models.Entities;
namespace MenuBackend.Models.ResponseModel;
public class GetCategoryResponse
{
    public int Count { get; set; } = 0;
    public List<Category> Items { get; set; } = new List<Category>();
}
