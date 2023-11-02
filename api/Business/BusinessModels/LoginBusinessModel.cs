using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.BusinessModels
{
    public class LoginBusinessModel
    {
        public string? Token { get; set; }
        public UserBusinessModel? User { get; set; }
    }
}
