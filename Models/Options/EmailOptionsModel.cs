namespace MenuBackend.Models.Options
{
    public class EmailOptionsModel
    {
        public string? Host { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public bool Secure { get; set; }
        public int Port { get; set; }
        public string? Name { get; set; }
    }
}
