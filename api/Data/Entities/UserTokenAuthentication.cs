using Data.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class UserTokenAuthentication : Entity
    {
        [ForeignKey("User")]
        public long UserId { get; set; }
        public virtual User? User { get; set; }
        public string? Token { get; set; }
        public bool? IsValid { get; set; }   
    }
}
