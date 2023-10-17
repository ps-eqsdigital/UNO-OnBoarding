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

namespace Business.BusinessObjects
{
    public class UserBusinessObject : AbstractBusinessObject, IUserBusinessObject
    {
        private readonly IGenericDataAccessObject _genericDataAccessObject;

        public UserBusinessObject(IGenericDataAccessObject genericDataAccessObject) {
            _genericDataAccessObject = genericDataAccessObject;
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

        public async Task<OperationResult<CreateUserBusinessModel>> Insert(User record)
        {
            return await ExecuteOperation(async () =>
            {
                if (!IsValidEmail(record.Email!) | record.Name.IsNullOrEmpty() | record.Picture.IsNullOrEmpty() | record.Phone.IsNullOrEmpty())
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

        private void sendEmail(string email, string password) {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("unoonboarding@sapo.pt");
            mail.To.Add(email);
            mail.Subject = "User password";
            mail.Body = "Your password is " + password;

           
            // Create a SMTP client and send the email
            SmtpClient smtpClient = new SmtpClient("smtp.sapo.pt");
            smtpClient.Port = 587;
            smtpClient.Credentials = new NetworkCredential("unoonboarding@sapo.pt", "Eqs_2023");
            smtpClient.EnableSsl = true;

            smtpClient.Send(mail);
        }
        static bool IsValidEmail(string email)
        {
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            Regex regex = new Regex(pattern);

            return regex.IsMatch(email);
        }
    }
}
