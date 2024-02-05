using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;

namespace WebApp.Utility
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Execute(email,subject, htmlMessage);
        }
        public async Task Execute(string email, string subject, string body)
        {
            var from = "dmitry.ermak2017@yandex.ru";
            var key = "bcgagodkyopzgpjg";

            var message = new MailMessage();
            message.From = new MailAddress(from, "Jewelry Store");
            message.To.Add(email);
            message.Subject = subject;
            message.IsBodyHtml = true;
            message.Body = body;

            using SmtpClient client = new SmtpClient("smtp.yandex.ru",587);
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential(from, key);
            

            client.Send(message);

        }
    }
}
