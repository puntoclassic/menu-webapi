using System.Text.Json.Serialization;
namespace MenuBackend.Models.Entities
{
    public class OrderState
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string? CssBadgeClass { get; set; }
    }
}
