namespace MenuBackend.Models.Entities
{
    public class CartItem
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public float Price { get; set; } = 0;
    }
}
