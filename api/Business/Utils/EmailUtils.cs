using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Business.Utils
{
    public class EmailUtils
    {
        public static bool IsValidEmail(string email)
        {
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            Regex regex = new Regex(pattern);

            return regex.IsMatch(email);
        }

        public void sendEmail(string email,string subject, string content)
        {
            try
            {
                IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

                string? smtpServer = configuration["SmtpConfig:SmtpServer"];
                string? smtpPortString = configuration["SmtpConfig:SmtpPort"];
                string? smtpUsername = configuration["SmtpConfig:SmtpUsername"];
                string? smtpPassword = configuration["SmtpConfig:SmtpPassword"];
                int smtpPort = int.Parse(smtpPortString!);

                SmtpClient client = new SmtpClient(smtpServer, smtpPort);
                client.EnableSsl = true;

                client.Credentials = new NetworkCredential(smtpUsername, smtpPassword);

                MailMessage message = new MailMessage();
                message.From = new MailAddress(smtpUsername!);
                message.To.Add(email);
                message.Subject = subject;
                message.Body = content;

                client.Send(message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error sending email");
            }
        }
    }
}
