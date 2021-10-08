using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPIFicheros.Models
{
    public class Login: EntityBase
    {
        public string User { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string AccessToken { get; set; }
    }
}
