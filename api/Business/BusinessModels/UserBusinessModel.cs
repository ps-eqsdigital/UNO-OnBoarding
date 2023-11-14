using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.BusinessModels
{
    public class UserBusinessModel
    {
        public Guid Uuid { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Picture {  get; set; }
        public UserBusinessModel() { }

        public UserBusinessModel(User user)
        {
            Uuid = user.Uuid;
            Name=user.Name;
            Email=user.Email;
            Picture=user.Picture;
        }

    }
}
