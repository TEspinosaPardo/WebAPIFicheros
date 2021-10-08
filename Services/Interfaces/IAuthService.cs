using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPIFicheros.Models.Dtos;

namespace WebAPIFicheros.Services.Interfaces
{
    public interface IAuthService
    {
        void Login(AuthDTO login);
        void Signup(AuthDTO login);
        void ForgotPassword(AuthDTO login);
    }
}
