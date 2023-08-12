namespace MenuBackend.Models.InputModel
{
    public class PostAccountActivateByTokenRequest
    {
        public string? Token { get; set; }
        public string? Email { get; set; }
    }
}
