using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using WebAPIFicheros.Models.Daos.Interfaces;
using WebAPIFicheros.Models.Dtos;

namespace WebAPIFicheros.Models.Daos
{
    public class AuthDAO : IAuthDAO
    {
        private readonly WebAPIFileContext db = new WebAPIFileContext();

        public void Login(AuthDTO login)
        {
            throw new NotImplementedException();
        }

        public void Add(Login login)
        {
            db.Logins.Add(login);
            db.SaveChanges();
        }

        public Login GetLoginByEmail(string email)
        {
            return db.Logins.FirstOrDefault(login => login.Email.Equals(email));
        }

        public bool CheckIfUserExists(string user, string email)
        {
            return db.Logins.FirstOrDefault(login => login.User.Equals(user) && login.Email.Equals(email)) != null;
        }

        public void Save(Login login)
        {
            db.SaveChanges();
        }
    }
}
