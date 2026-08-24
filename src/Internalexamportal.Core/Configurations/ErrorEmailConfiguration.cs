
namespace Internalexamportal.Core.Configurations
{
    public class ErrorEmailConfiguration
    {
        public bool SendErrorEmail { get; set; }
        public string SmtpServerName { get; set; }
        public string ErrorEmailSender { get; set; }
        public int SmtpPortNumber { get; set; }
        public string ErrorEmailSenderUsername { get; set; }
        public string ErrorEmailSenderPassword { get; set; }
        public string[] ErrorEmailRecepientList { get; set; }
    }
}
