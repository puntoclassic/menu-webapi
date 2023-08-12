using System.Text.Json.Serialization;
using MenuBackend.Models.Auth;

namespace MenuBackend.Models.Entities
{

    public class OrderDetailDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Quantity { get; set; }
        public float UnitPrice { get; set; }

    }
}
