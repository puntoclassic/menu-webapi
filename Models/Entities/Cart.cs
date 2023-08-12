namespace MenuBackend.Models.Entities
{
    public class Cart
    {
        public Dictionary<string, CartRow>? Items { get; set; }
        public decimal Total { get; set; }
        public string TipologiaConsegna { get; set; } = "asporto";
        public string? Indirizzo { get; set; }
        public string? Orario { get; set; }
        public string? Note { get; set; }
    }
}
