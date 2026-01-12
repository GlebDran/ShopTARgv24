using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using ShopTARgv24.Core.Dto;
using ShopTARgv24.Core.ServiceInterface;



namespace ShopTARgv24.ApplicationServices.Services
{
    public class EmailServices : IEmailServices
    {
        private readonly IConfiguration _config;

        public EmailServices
            (
                IConfiguration config
            )
        {
            _config = config;
        }

        public void SendEmail(EmailDto dto)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config.GetSection("EmailUserName").Value));
            email.To.Add(MailboxAddress.Parse(dto.To));
            email.Subject = dto.Subject;

            var builder = new BodyBuilder
            {
                HtmlBody = dto.Body
            };

            //vaja teha foreach, kus saab lisada mitu faili
            //vaja kasutada kontrolli, kus kui faili pole, siis ei lisa
            foreach (var file in dto.Attachment)
            {
                if (file.Length > 0)
                {
                    using (var stream = new MemoryStream())
                    {
                        file.CopyTo(stream);
                        stream.Position = 0;
                        builder.Attachments.Add(file.FileName, stream.ToArray());
                    }
                }
            }
            email.Body = builder.ToMessageBody();


            using var smtp = new SmtpClient();

            smtp.Connect(_config.GetSection("EmailHost").Value, 587, SecureSocketOptions.StartTls);
            smtp.Authenticate(_config.GetSection("EmailUserName").Value, _config.GetSection("EmailPassword").Value);
            smtp.Send(email);
            smtp.Disconnect(true);
        }


        public void SendEmailToken(EmailTokenDto dto, string token)
        {
            dto.Token = token;
            var email = new MimeMessage();

            email.From.Add(MailboxAddress.Parse(_config.GetSection("EmailUserName").Value));
            email.To.Add(MailboxAddress.Parse(dto.To));
            email.Subject = dto.Subject;
            var builder = new BodyBuilder
            {
                HtmlBody = dto.Body
            };

            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();

            smtp.Connect(_config.GetSection("EmailHost").Value, 587, SecureSocketOptions.StartTls);
            smtp.Authenticate(_config.GetSection("EmailUserName").Value, _config.GetSection("EmailPassword").Value);
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}