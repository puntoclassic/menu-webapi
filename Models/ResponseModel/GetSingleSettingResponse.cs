using System.Text.Json.Serialization;
namespace MenuBackend.Models.ResponseModel
{
    public class SettingPublicReponse
    {
        public string Key { get; set; } = "";
        public string? Value { get; set; }
    }
}
