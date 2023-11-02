using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Business.Utils
{
    public class EmailUtils
    {
        public void sendEmail(string email,string subject, string content)
        {
            try
            {
                IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

                var smtpServer = configuration["SmtpConfig:SmtpServer"];
                var smtpPortString = configuration["SmtpConfig:SmtpPort"];
                var smtpUsername = configuration["SmtpConfig:SmtpUsername"];
                var smtpPassword = configuration["SmtpConfig:SmtpPassword"];
                int smtpPort = int.Parse(smtpPortString);

                SmtpClient client = new SmtpClient(smtpServer, smtpPort);
                client.EnableSsl = true;

                client.Credentials = new NetworkCredential(smtpUsername, smtpPassword);

                MailMessage message = new MailMessage();
                message.From = new MailAddress(smtpUsername);
                message.To.Add(email);
                message.Subject = subject;
                message.Body = content;

                client.Send(message);
                Console.WriteLine("Email sent successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error sending email: " + ex.Message);
            }
        }
    }
}
