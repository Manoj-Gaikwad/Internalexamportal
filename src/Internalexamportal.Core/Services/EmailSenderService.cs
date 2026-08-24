using Internalexamportal.Core.Commons;
using Internalexamportal.Core.Configurations;
using Internalexamportal.Core.Features.Admin.testManagement;
using Internalexamportal.Core.FileSystem;
using MailKit.Security;
using Microsoft.AspNetCore.Hosting.Server;
//using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using MimeKit.Text;
using Internalexamportal.Core.Models;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Internalexamportal.Core.Services
{
    public class EmailSenderService : IEmailSenderService
    {
        private readonly ConfirmationEmailConfiguration _emailConfiguration;
        private readonly FileConfiguration _fileConfiguraiton;
        private readonly IHttpContextAccessor _contextAccessor;


        public EmailSenderService(
            IOptions<ConfirmationEmailConfiguration> emailConfiguration,
            IOptions<FileConfiguration> fileConfiguration,
            IHttpContextAccessor contextAccessor
        )
        {
            _emailConfiguration = emailConfiguration.Value;
            _fileConfiguraiton = fileConfiguration.Value;
            _contextAccessor = contextAccessor;
        }

        public async Task SendConfirmationEmail(string email, string message)
        {
            string MailText = GetTemplate(Templates.MessageEmail);

            if (string.IsNullOrEmpty(MailText)) return;

            MailText = "<p>Welcome to Vidyaops Exam Portal</p>" +
                        "<p>Please click on the following link to activate your account.</p>" +
                        "<br />" + message;

            Notification notification = new Notification
            {
                Body = MailText,
                From = _emailConfiguration.ConfirmationEmailSender,
                Subject = "Welcome to Exam Portal",
                To = email.Split(',').ToList()
            };

            await Send(notification);
        }

        public async Task SendPasswordEmail(string email, string message)
        {
            string MailText = GetTemplate(Templates.MessageEmail);

            if (string.IsNullOrEmpty(MailText)) return;

            MailText = MailText.Replace("[Title]", "Login Credentials")
                                .Replace("[Message]", ExamLinkTemplate.PasswordEmailTemplate
                                    .Replace("@Password", message)
                                    .Replace("@url", GetBaseUrl())
                                );

            Notification notification = new Notification
            {
                Body = MailText,
                From = _emailConfiguration.ConfirmationEmailSender,
                Subject = "Exam Portal Login Credentials",
                To = email.Split(',').ToList()
            };

            await Send(notification);

        }

        public async Task SendForgotPasswordLink(string email, string message)
        {
            string MailText = GetTemplate(Templates.MessageEmail);

            if (string.IsNullOrEmpty(MailText)) return;

            MailText = MailText.Replace("[Title]", "Reset Password")
                                .Replace("[Message]", EmailBodyTemplates.forgotPasswordTemplate
                                .Replace("@here", message));

            Notification notification = new Notification
            {
                Body = MailText,
                From = _emailConfiguration.ConfirmationEmailSender,
                Subject = "Reset Password Link",
                To = email.Split(',').ToList()
            };

            await Send(notification);
        }

        public async Task SendExamLink(string email, string message, string url)
        {
            string MailText = GetTemplate(Templates.MessageEmail);

            if (string.IsNullOrEmpty(MailText)) return;

            MailText = MailText.Replace("[Title]", "Exam Code")
                            .Replace("[Message]", ExamLinkTemplate.ExamTemplate
                            .Replace("@Activation", message).Replace("@url", url));

            Notification notification = new Notification
            {
                Body = MailText,
                From = _emailConfiguration.ConfirmationEmailSender,
                Subject = "Exam Link",
                To = email.Split(',').ToList()
            };

            await Send(notification);

        }

        public async Task SendExamResult(SendExamResultEmailModel model, string url)
        {
            string MailText = GetTemplate(Templates.ExamResultEmail);

            if (string.IsNullOrEmpty(MailText)) return;

            MailText = MailText.Replace("[TestName]", model.TestName)
                            .Replace("[Name]", model.UserName)
                            .Replace("[TestDate]", model.SubmitDate)
                            .Replace("[ClientName]", model.ClientName)
                            .Replace("[WebsiteUrl]", url)
                            .Replace("[ClientEmail]", model.ClientEmail);

            Notification notification = new Notification
            {
                Body = MailText,
                From = _emailConfiguration.ConfirmationEmailSender,
                Subject = "Exam Result of " + model.TestName,
                To = model.Email.Split(',').ToList()
            };

            await Send(notification);
        }

        #region Internal
        private string GetTemplate(string template)
        {
            string FilePath = _fileConfiguraiton.TemplateBaseFolderPath + "/" + template + FileExtension.HTML;
            string MailText = string.Empty;
            if (File.Exists(FilePath))
            {
                StreamReader str = new StreamReader(FilePath);
                MailText = str.ReadToEnd();
                str.Close();
                return MailText;
            }
            else
            {
                return MailText;
            }
        }

        private async Task Send(Notification message)
        {
            if ((message?.To?.Count ?? 0) <= 0) return;

            using (SmtpClient smtpClient = GetSmtpClient())
            {
                foreach (var email in message.To)
                {
                    MailMessage mailMessage = CreateMailMessage(message, email);

                    try
                    {
                        await smtpClient.SendMailAsync(mailMessage);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                        throw ex;
                    }

                    Console.WriteLine($" [x] Sent Email to {email}");
                }
            }
        }

        private MailMessage CreateMailMessage(Notification message, string email)
        {
            var mailMessage = new MailMessage
            {
                Subject = message.Subject,
                IsBodyHtml = true,
                Body = message.Body,
                Priority = MailPriority.Normal,
                From = new MailAddress(message.From, "Vidyaops Exam Portal"),
            };

            // recipient
            mailMessage.To.Add(new MailAddress(email));

            //attachments
            var attachments = message.Attachment;
            if ((attachments?.Count ?? 0) <= 0) return mailMessage;

            foreach (var item in attachments)
            {
                if (item != null)
                {
                    mailMessage.Attachments.Add(new Attachment(item.File, item.Name));
                }
            }

            return mailMessage;
        }

        private SmtpClient GetSmtpClient()
        {
            var smtp = new SmtpClient
            {
                Port = _emailConfiguration.SmtpPortNumber,
                Host = _emailConfiguration.SmtpServerName,
                EnableSsl = true,
                UseDefaultCredentials = false
            };
            smtp.Credentials = new NetworkCredential(_emailConfiguration.ConfirmationEmailSenderUsername, _emailConfiguration.ConfirmationEmailSenderPassword);
            return smtp;
        }

        private string GetBaseUrl()
        {
            var url = $"{_contextAccessor.HttpContext.Request.Scheme}://" +
                $"{_contextAcc‌​essor.HttpContext.Re‌​quest.Host.ToUriComp‌​onent()}" +
                $"{_contextAcc‌​essor.HttpContext.Re‌​quest.PathBase}";

            return url;
        }

        #endregion
    }
}
