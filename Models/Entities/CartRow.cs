namespace MenuWebapi.Models.Entities
{
    public class CartRow
    {
        public int Quantity { get; set; }
        public CartItem? Item { get; set; }
    }
}
