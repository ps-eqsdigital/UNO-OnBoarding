using Data.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class User : Entity
    {
        public string? Name { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public string? Phone {  get; set; }
        public string? Picture { get; set; }
        public string? PasswordResetToken { get; set; }
        public DateTime PasswordResetTokenExpiration { get; set; }
        public Role Role { get; set; }
    }
}
