using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPIFicheros.Models.Dtos;

namespace WebAPIFicheros.Models.Daos.Interfaces
{
    public interface IAuthDAO
    {
        void Login(AuthDTO login);
        void Add(Login login);
        Login GetLoginByEmail(string email);
    }
}
