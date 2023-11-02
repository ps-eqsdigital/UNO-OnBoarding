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
using System.Globalization;
using Business.Utils;

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
                    throw new Exception("user does not exist");
                }
                if (!IsValidEmail(record.Email!) || record.Name.IsNullOrEmpty() || record.Picture.IsNullOrEmpty() || record.Phone.IsNullOrEmpty() || record.Password!.Length < 8 )
                {
                    throw new Exception();
                }
                else
                {
                    user.Name = record.Name;
                    user.Password = record.Password;
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
                    throw new Exception();
                }
                List<User> result =await _userDataAccessObject.FilterUsers(search,sort);
                return result;

            });
        }
        public async Task<OperationResult<CreateUserBusinessModel>> Insert(User record)
        {
            return await ExecuteOperation(async () =>
            {
                if (!IsValidEmail(record.Email!) || record.Name.IsNullOrEmpty() || record.Picture.IsNullOrEmpty() || record.Phone.IsNullOrEmpty())
                {
                    Console.WriteLine("ola");
                    throw new Exception();
                }
                string password = GenerateRandomPassword();
                record.Password = password;
                EmailUtils emailUtils = new EmailUtils();
                string subject = "Password";
                emailUtils.sendEmail(record.Email!,subject, password);
                Console.Write(record);
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

        static bool IsValidEmail(string email)
        {
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            Regex regex = new Regex(pattern);

            return regex.IsMatch(email);
        }

    }
}
