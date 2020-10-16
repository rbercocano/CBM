using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Linq;
using Microsoft.Extensions.Options;
using System.IO;

namespace Charcutarie.Core.SMTP
{
    public class EmailManager : IEmailManager
    {
        private readonly SMTPSettings settings;
        public EmailManager(IOptions<SMTPSettings> options)
        {
            settings = options.Value;
        }
        public void SendEmail(List<string> to, string body, string subject, bool isHtml = false)
        {
            var client = new SmtpClient(settings.Server)
            {
                Credentials = new NetworkCredential(settings.UserName, settings.Password),
                Port = settings.Port,
                EnableSsl = true
            };

            MailMessage mailMessage = new MailMessage
            {
                From = new MailAddress(settings.From, settings.FromDisplayName),
                Body = body,
                Subject = subject,
                IsBodyHtml = isHtml
            };
            to.ForEach(t => mailMessage.To.Add(t));
            client.Send(mailMessage);
        }

        public void SendRegistrationEmail(string account, string username, string to, string company, int customerTypeId, string socialIdentifier)
        {
            var type = customerTypeId == 1 ? "CPF" : "CNPJ";
            var path = Path.Combine(Environment.CurrentDirectory, "Templates", "Registration.html");
            var body = File.ReadAllText(path);
            body = body.Replace("[account_number]", account)
                .Replace("[username]", username)
                .Replace("[name]", company)
                .Replace("[social_identifier_type]", type)
                .Replace("[social_identifier]", socialIdentifier);
            SendEmail(new List<string> { to }, body, "Bem vindo ao Charcuterie Business Manager", true);
        }
    }
}
