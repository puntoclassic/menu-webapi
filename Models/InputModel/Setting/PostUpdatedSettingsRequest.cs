namespace MenuWebapi.Models.InputModel
{
    public class PostUpdatedSettingsRequest
    {
        public string? SiteName { get; set; }
        public string? SiteSubtitle { get; set; }
        public float? ShippingCosts { get; set; }
        public int? OrderCreatedStateId { get; set; }
        public int? OrderPaidStateId { get; set; }
    }
}
