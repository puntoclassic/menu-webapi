using System.Text.Json.Serialization;
namespace MenuBackend.Models.Entities
{
    public class Setting
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string? SiteName { get; set; }
        public string? SiteSubtitle { get; set; }
        public float? ShippingCosts { get; set; }
        public int? OrderCreatedStateId { get; set; }
        [JsonIgnore]
        public OrderState? OrderCreatedState { get; set; }
        public int? OrderPaidStateId { get; set; }
        [JsonIgnore]
        public OrderState? OrderPaidState { get; set; }
    }
}
