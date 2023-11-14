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
using Azure.Identity;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

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
                if (!EmailUtils.IsValidEmail(record.Email!) || record.Name.IsNullOrEmpty() || record.Picture.IsNullOrEmpty() || record.Phone.IsNullOrEmpty() || record.Password!.Length < 8 )
                {
                    throw new Exception();
                }
                else
                {
                    user.Name = record.Name;
                    user.Password = record.Password;
                    user.Email = record.Email;
                    user.Picture = record.Picture;
                    user.Password = PasswordUtils.EncodePasswordToBase64(record.Password!);
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

                if (!EmailUtils.IsValidEmail(record.Email!) || record.Name.IsNullOrEmpty() || record.Picture.IsNullOrEmpty() || record.Phone.IsNullOrEmpty())
                {
                    throw new Exception();
                }

                User result = await _userDataAccessObject.GetUserByEmail(record.Email!);
                if (result != null)
                {
                   throw new Exception("Email already exists");
                }

                string password = PasswordUtils.GenerateRandomPassword();
                string subject = "Password";
                
                EmailUtils emailUtils = new EmailUtils();
                emailUtils.sendEmail(record.Email!,subject, password);

                record.Password = PasswordUtils.EncodePasswordToBase64(password);
                await _genericDataAccessObject.InsertAsync<User>(record);
                return new CreateUserBusinessModel { Uuid = record.Uuid};
            });
        }

        public async Task<OperationResult<LoginBusinessModel>> Login(string email, string password)
        {
            return await ExecuteOperation(async () =>
            {
                if (!EmailUtils.IsValidEmail(email))
                {
                    throw new Exception("Invalid email");
                }

                User result = await _userDataAccessObject.GetUserByEmail(email);
                if (result == null)
                {
                    throw new Exception("No email found");
                }
                if (PasswordUtils.DecodeFrom64(result.Password!) != password)
                {
                    throw new Exception("Invalid password");
                }
                string token = TokenUtils.CreateAuthenticationToken(result);

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
                var result = await _userDataAccessObject.GetTokenUuidByToken(token);
                if (result == null)
                {
                    throw new Exception("Failed to logout");
                }
                result!.IsValid = false;
                await _genericDataAccessObject.UpdateAsync(result);

            });
        }

        public async Task<OperationResult> RecoverPassword(string email)
        {
            return await ExecuteOperation(async () =>
            {
                User user = await _userDataAccessObject.GetUserByEmail(email);

                if (user == null)
                {
                    throw new Exception("User doesnt exist");
                }
                else
                {
                    string resetToken = TokenUtils.GeneratePasswordResetToken();

                    user.PasswordResetToken = resetToken;
                    user.PasswordResetTokenExpiration = DateTime.UtcNow.AddHours(1); 

                    await _genericDataAccessObject.UpdateAsync(user);

                    EmailUtils emailUtils = new EmailUtils();
                    string subject = "Password Reset Token";
                    emailUtils.sendEmail(email!, subject, resetToken);
                }
            });
        } 

        public async Task<OperationResult> ResetPassword(string passwordResetToken, string newPassword)
        {
            return await ExecuteOperation(async () =>
            {
                User user = await _userDataAccessObject.GetUserByPasswordResetToken(passwordResetToken);

                if (user == null)
                {
                    throw new Exception("Invalid token");
                }
                if (DateTime.UtcNow > user.PasswordResetTokenExpiration)
                {
                    throw new Exception("Token expired");
                }
                string userPassword= PasswordUtils.DecodeFrom64(user.Password!);
                if (string.IsNullOrEmpty(newPassword) || newPassword == userPassword) {
                    throw new Exception("Please insert a valid password");
                }

                user.Password = PasswordUtils.EncodePasswordToBase64(newPassword);
                user.PasswordResetTokenExpiration = DateTime.UtcNow;
                await _genericDataAccessObject.UpdateAsync(user);
            });
        }
    }
}
