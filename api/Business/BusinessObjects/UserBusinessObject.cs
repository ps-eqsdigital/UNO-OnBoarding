using Business.Base;
using Business.Interfaces;
using Data.Entities;
using DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using Business.BusinessModels;
using System.Text.RegularExpressions;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Protocols;
using Microsoft.Extensions.Configuration;

namespace Business.BusinessObjects
{
    public class UserBusinessObject : AbstractBusinessObject, IUserBusinessObject
    {
        private readonly IGenericDataAccessObject _genericDataAccessObject;
        private readonly IUserDataAccessObject _userDataAccessObject;
        public UserBusinessObject(IGenericDataAccessObject genericDataAccessObject, IUserDataAccessObject userDataAccessObject)
        {
            _genericDataAccessObject = genericDataAccessObject;
            _userDataAccessObject = userDataAccessObject;
        }

        public async Task<OperationResult> Update(Guid uuid, User record)
        {
            return await ExecuteOperation(async () =>
            {
                var user = await _genericDataAccessObject.GetAsync<User>(uuid);

                if (user == null)
                {
                    throw new Exception("user doesn't exist");
                }
                else
                {
                    user.Name = record.Name;
                    user.Email = record.Email;
                    user.Picture = record.Picture;
                    user.Password = record.Password;
                    user.Phone = record.Phone;
                    user.Role = record.Role;
                }
                await _genericDataAccessObject.UpdateAsync<User>(user);
            });
        }

        public async Task<OperationResult<List<User>>> ListFilteredUsers(string search, int sort)
        {
            return await ExecuteOperation(async () => {

                if (sort !=0 && sort != 1)
                {
                    Console.WriteLine("ex");
                    throw new Exception();
                }
                var result =await _userDataAccessObject.FilterUsers(search,sort);
                return result;

            });
        }
        public async Task<OperationResult<CreateUserBusinessModel>> Insert(User record)
        {
            return await ExecuteOperation(async () =>
            {
                if (!IsValidEmail(record.Email!) || record.Name.IsNullOrEmpty() || record.Picture.IsNullOrEmpty() || record.Phone.IsNullOrEmpty())
                {
                    throw new Exception();
                }
                string password = GenerateRandomPassword();
                record.Password = password;
                sendEmail(record.Email!, password);

                await _genericDataAccessObject.InsertAsync<User>(record);
                return new CreateUserBusinessModel { Uuid = record.Uuid};
            });
        }

        private string GenerateRandomPassword(int length = 12)
        {
            string lowercaseLetters = "abcdefghijklmnopqrstuvwxyz";
            string uppercaseLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string digits = "0123456789";
            string specialCharacters = "!@#$%^&*()_-+=<>?/\\";

            string allCharacters = lowercaseLetters + uppercaseLetters + digits + specialCharacters;

            length = Math.Max(length, 8);

            Random random = new Random();
            StringBuilder passwordBuilder = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                int index = random.Next(allCharacters.Length);
                passwordBuilder.Append(allCharacters[index]);
            }

            return passwordBuilder.ToString();
        }

        private void sendEmail(string email, string password)
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

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("unoonboarding@sapo.pt");
            mail.To.Add(email);
            mail.Subject = "User password";
            mail.Body = "Your password is " + password;

            SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort);
            smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
            smtpClient.EnableSsl = true;

            try
            {
                smtpClient.Send(mail);
                Console.WriteLine("Email sent successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error sending email: " + ex.Message);
            }
        }
        static bool IsValidEmail(string email)
        {
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            Regex regex = new Regex(pattern);

            return regex.IsMatch(email);
        }
    }
}
