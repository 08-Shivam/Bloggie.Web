namespace Blogspot.Models.ViewModel.Email
{
    public class GetEmailSettings
    {
        public string SecretKey {  get; set; }=default;

        public string From { get; set; } = default;

        public string SmtpServer { get; set; } = default;

        public int Port {  get; set; }=default ;

        public bool EnableSSL { get; set; } = default;
    }
}
