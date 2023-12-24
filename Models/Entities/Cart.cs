namespace MenuWebapi.Models.Entities
{
    public class Cart
    {
        public Dictionary<string, CartRow>? Items { get; set; }
        public decimal Total { get; set; }
        public string DeliveryType { get; set; } = "asporto";
        public string? DeliveryAddress { get; set; }
        public string? DeliveryTime { get; set; }
        public string? Notes { get; set; }
        public int CarrierId { get; set; }
    }
}
