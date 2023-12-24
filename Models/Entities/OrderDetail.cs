using System.Text.Json.Serialization;
using MenuWebapi.Models.Auth;

namespace MenuWebapi.Models.Entities
{

    public class OrderDetail
    {
        public int Id { get; set; }
        [JsonIgnore]
        public int? OrderId { get; set; }
        [JsonIgnore]
        public Order? Order { get; set; }
        public string? Name { get; set; }
        public int Quantity { get; set; }
        public float UnitPrice { get; set; }
    }
}
