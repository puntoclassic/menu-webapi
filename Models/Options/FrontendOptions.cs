namespace MenuBackend.Models.Options
{
    public class FrontendOptions
    {
        public string? BaseUrl { get; set; }
        public string? ActivateAccountByTokenUrl { get; set; }
        public string? ResetPasswordByTokenUrl { get; set; }
        public string? PaymentSuccessUrl { get; set; }
        public string? PaymentFailedUrl { get; set; }
    }
}
