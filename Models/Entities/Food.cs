namespace MenuWebapi.Models.Entities
{
    public class Food
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string? Ingredients { get; set; }
        public float Price { get; set; } = 0;
        public int? CategoryId { get; set; }
        public Category? Category { get; set; }
        public bool Deleted { get; set; } = false;

    }
}
