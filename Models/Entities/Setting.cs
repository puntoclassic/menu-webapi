using System.Text.Json.Serialization;
namespace MenuWebapi.Models.Entities
{
    public class Setting
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string? SiteName { get; set; } = "Demo";
        public string? SiteSubtitle { get; set; } = "Ristorante | Pizzeria";
        public int? OrderCreatedStateId { get; set; }
        [JsonIgnore]
        public OrderState? OrderCreatedState { get; set; }
        public int? OrderPaidStateId { get; set; }
        [JsonIgnore]
        public OrderState? OrderPaidState { get; set; }
        public int? OrderDeletedStateId { get; set; }
        [JsonIgnore]
        public OrderState? OrderDeletedState { get; set; }
    }
}
