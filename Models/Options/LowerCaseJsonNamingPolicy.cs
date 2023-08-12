using System.Text.Json;
namespace MenuBackend.Models.Options
{
    public class LowerCaseJsonNamingPolicy : JsonNamingPolicy
    {
        public override string ConvertName(string name) =>
            name.ToLower();
    }
}
