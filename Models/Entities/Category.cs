using System.Text.Json.Serialization;
namespace MenuWebapi.Models.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string? Slug { get; set; }
        public string? ImageUrl { get; set; }
        [JsonIgnore]
        public ICollection<Food>? Foods { get; set; }
        public bool Deleted { get; set; } = false;
    }
}
