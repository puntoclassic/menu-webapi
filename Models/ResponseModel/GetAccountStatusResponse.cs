namespace MenuBackend.Models.ResponseModel
{
    public class GetAccountStatusResponse
    {
        public string? Status { get; set; }
        public bool Logged { get; set; } = false;
        public LoggedUser? User { get; set; }
    }
}
