using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Threading.Tasks;
using WebApiEcomm.Core.Entites.Dtos;
using WebApiEcomm.Core.Services;

namespace WebApiEcomm.InfraStructure.Repositores.Service
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmail(EmailDto emailDto)
        {
            var mimeMessage = new MimeMessage();
            mimeMessage.From.Add(new MailboxAddress("My Ecom", _configuration["EmailSetting:From"]));
            mimeMessage.To.Add(new MailboxAddress(emailDto.To, emailDto.To));
            mimeMessage.Subject = emailDto.Subject;
            mimeMessage.Body = new TextPart("html")
            {
                Text = emailDto.Content
            };

            using (var smtp = new MailKit.Net.Smtp.SmtpClient())
            {
                try
                {
                    await smtp.ConnectAsync(
                        _configuration["EmailSetting:Smtp"],
                        int.Parse(_configuration["EmailSetting:Port"]),
                        true);

                    await smtp.AuthenticateAsync(
                        _configuration["EmailSetting:UserName"],
                        _configuration["EmailSetting:Password"]);

                    await smtp.SendAsync(mimeMessage);
                }
                catch (Exception)
                {
                    throw new Exception("Email could not be sent. Please check your email settings.");
                }
                finally
                {
                    await smtp.DisconnectAsync(true);
                }
            }
        }
    }
}
