using Asp_Net_Good_idea.Models.EmailModel;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System;
namespace Asp_Net_Good_idea.UtilityService.EmailService

{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration configuration)
        {
            _config = configuration;
        }



        public void SendEmail(EmailModel emailModel)
        {
            var emailMessage = new MimeMessage();
            var from = _config["EmailSettings:From"];
            emailMessage.From.Add(new MailboxAddress("TE Connectivity", from));
            emailMessage.To.Add(new MailboxAddress(emailModel.To, emailModel.To));
            emailMessage.Subject = emailModel.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {

                Text = emailModel.Content



            };
            using (var client = new SmtpClient())
            {
                try
                {

                    client.Connect(_config["EmailSettings:SmtpServer"], 587, SecureSocketOptions.StartTls);

                    client.Authenticate(_config["EmailSettings:From"], _config["EmailSettings:Password"]);
                    client.Send(emailMessage);

                }
                catch (Exception ex) { throw; }

                finally
                {

                    client.Disconnect(true);
                    client.Dispose();

                }

            }


        }
    }
}
