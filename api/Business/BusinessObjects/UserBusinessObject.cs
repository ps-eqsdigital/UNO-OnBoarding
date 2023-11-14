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
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace Business.BusinessObjects
{
    public class UserBusinessObject : AbstractBusinessObject, IUserBusinessObject
    {
        private readonly IGenericDataAccessObject _genericDataAccessObject;
        private readonly IUserDataAccessObject _userDataAccessObject;
        private readonly IConfiguration _configuration;

        public UserBusinessObject(IGenericDataAccessObject genericDataAccessObject, IUserDataAccessObject userDataAccessObject, IConfiguration configuration)
        {
            _genericDataAccessObject = genericDataAccessObject;
            _userDataAccessObject = userDataAccessObject;
            _configuration = configuration;
        }

        public async Task<OperationResult<List<UserBusinessModel>>> GetUsers()
        {
            return await ExecuteOperation(async () =>
            {
                List<User> users = await _genericDataAccessObject.ListAsync<User>();
                var result = users.Select(u => new UserBusinessModel(u)).ToList();
                return result;
            });

        }
        public async Task<OperationResult> Update(Guid uuid, User record)
        {
            return await ExecuteOperation(async () =>
            {
                User? user = await _genericDataAccessObject.GetAsync<User>(uuid);

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
                    user.Password = EncodePasswordToBase64(record.Password);
                    user.Phone = record.Phone;
                    user.Role = record.Role;
                }
                await _genericDataAccessObject.UpdateAsync<User>(user);
            });
        }

        public async Task<OperationResult<List<UserBusinessModel>>> ListFilteredUsers(string search, int sort)
        {
            return await ExecuteOperation(async () => {

                if (sort !=0 && sort != 1)
                {
                    throw new Exception();
                }

                List<User> users =await _userDataAccessObject.FilterUsers(search,sort);
                var result = users.Select(u => new UserBusinessModel(u)).ToList();

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

                User result = await _userDataAccessObject.GetUserByEmail(record.Email!);
                if (result != null)
                {
                   throw new Exception("Email already exists");
                }

                string password = GenerateRandomPassword();
                EmailUtils emailUtils = new EmailUtils();
                string subject = "Password";
                emailUtils.sendEmail(record.Email!,subject, password);

                record.Password = EncodePasswordToBase64(password);
                await _genericDataAccessObject.InsertAsync<User>(record);
                return new CreateUserBusinessModel { Uuid = record.Uuid};
            });
        }

        public async Task<OperationResult<LoginBusinessModel>> Login(string email, string password)
        {
            return await ExecuteOperation(async () =>
            {
                if (!IsValidEmail(email))
                {
                    throw new Exception("Invalid email");
                }

                User result = await _userDataAccessObject.GetUserByEmail(email);
                if (result == null)
                {
                    throw new Exception("No email found");
                }
                if (DecodeFrom64(result.Password!) != password)
                {
                    throw new Exception("Invalid password");
                }
                string token = CreateToken(result);

                UserTokenAuthentication userToken = await _userDataAccessObject.GetUserTokenByUserId(result.Id);

                if(userToken == null)
                {
                    userToken = new UserTokenAuthentication
                    {
                        UserId = result.Id,
                        Token = token,
                        IsValid = true
                    };
                }
                else
                {
                    userToken.Token = token;
                    userToken.IsValid = true;
                }

                await _genericDataAccessObject.UpdateAsync<UserTokenAuthentication>(userToken);

                return new LoginBusinessModel { Token = token, User = new UserBusinessModel {
                    Email=result.Email, Name=result.Name, Picture=result.Picture, Uuid=result.Uuid
                    } 
                };
            });
        }

        public async Task<OperationResult> Logout(string token)
        {
            return await ExecuteOperation(async () =>
            {
                var result = await _userDataAccessObject.GetToken(token);
                if (result == null)
                {
                    throw new Exception("Failed to logout");
                }
                result!.IsValid = false;
                await _genericDataAccessObject.UpdateAsync(result);

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

        private string CreateToken(User user)
        {
            IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Name!),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("AppSettings:Token").Value!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                claims:claims,
                expires:DateTime.Now.AddDays(1),
                signingCredentials:creds
                );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }

        public static string EncodePasswordToBase64(string password)
        {
            try
            {
                byte[] encData_byte = new byte[password.Length];
                encData_byte = System.Text.Encoding.UTF8.GetBytes(password);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in base64Encode" + ex.Message);
            }
        }

        public string DecodeFrom64(string encodedData)
        {
            System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
            System.Text.Decoder utf8Decode = encoder.GetDecoder();
            byte[] todecode_byte = Convert.FromBase64String(encodedData);
            int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
            char[] decoded_char = new char[charCount];
            utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
            string result = new String(decoded_char);
            return result;
        }
    }
}
