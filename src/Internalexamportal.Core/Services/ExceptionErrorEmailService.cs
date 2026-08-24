using System;
using System.Security.Principal;
using Internalexamportal.Core.Configurations;
using Internalexamportal.Core.Commons;
using MimeKit;
using MailKit.Net.Smtp;

namespace Internalexamportal.Core.Services
{
    public class ExceptionErrorEmailService : IExceptionEmailSenderService
    {
        public void SendEmail(
            Exception exception,
            ErrorEmailConfiguration errorEmailConfiguration,
            IPrincipal principal
        )
        {
            //Get user id from principal or set it to null of no users
            var userId = !String.IsNullOrEmpty(principal.Identity.GetUserId()) ?
                principal.Identity.GetUserId() : "Not Authenticated";

            //Declare Email Title
            string fromAdressTitle = "Vidhyaops";

            //Decalre Email Subject
            string subject = "Vidhaops Exception";

            //Declare error message and configue sender, recepients and email body
           
            var mimeMessage = new MimeMessage();
            mimeMessage.From.Add(new MailboxAddress(fromAdressTitle,
                errorEmailConfiguration.ErrorEmailSender));

            //Add multiple recipients from app settings
            foreach (string recipientEmailAddress in
                errorEmailConfiguration.ErrorEmailRecepientList)
            {
                mimeMessage.To.Add(new MailboxAddress(recipientEmailAddress));
            }

            //Set Email subject
            mimeMessage.Subject = subject;

            //Send body with exception message, stacktrace and UserId
            mimeMessage.Body = new TextPart("plain")
            {
                Text = exception.Message
                       + Environment.NewLine
                       + Environment.NewLine
                       + "User: " + userId
                       + Environment.NewLine
                       + Environment.NewLine
                       + exception.StackTrace
            };

            //Configure smtp client to send email
            using (var client = new SmtpClient())
            {
                //Connect to the server with specified port and server name
                client.Connect(errorEmailConfiguration.SmtpServerName,
                    errorEmailConfiguration.SmtpPortNumber, false);

                //Aunticate with the specified credentials in app settings
                client.Authenticate(errorEmailConfiguration.ErrorEmailSenderUsername,
                    errorEmailConfiguration.ErrorEmailSenderPassword);

                //Send email using the server
                client.Send(mimeMessage);

                //Disconnect with the server
                client.Disconnect(true);
            }
        }
    }
}
