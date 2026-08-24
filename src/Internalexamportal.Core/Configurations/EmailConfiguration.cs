namespace Internalexamportal.Core.Configurations
{
    public class EmailConfiguration
    {
        public string SmtpServerName { get; set; }
        public string ConfirmationEmailSender { get; set; }
        public int SmtpPortNumber { get; set; }
        public string ConfirmationEmailSenderUsername { get; set; }
        public string ConfirmationEmailSenderPassword { get; set; }
        public string ConfirmationEmailUrl { get; set; }
        public string DisplayName { get; set; }
    }
}
