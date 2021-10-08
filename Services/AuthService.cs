using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WebAPIFicheros.Models;
using WebAPIFicheros.Models.Daos;
using WebAPIFicheros.Models.Daos.Interfaces;
using WebAPIFicheros.Models.Dtos;
using WebAPIFicheros.Services.Interfaces;
using System.Web;
using System.Net;

namespace WebAPIFicheros.Services
{
    public class AuthService: IAuthService
    {
        private readonly IAuthDAO authDAO = new AuthDAO();

        public void Login(AuthDTO login)
        {
            throw new NotImplementedException();
        }

        public void Signup(AuthDTO login)
        {
            if(!authDAO.CheckIfUserExists(login.Signup.Request.User, login.Signup.Request.Email))
            {
                var signUpLogin = new Login()
                {
                    User = login.Signup.Request.User,
                    Password = Encrypt(login.Signup.Request.Password),
                    Email = login.Signup.Request.Email
                };

                authDAO.Add(signUpLogin);
            }
            else
            {
                login.Signup.Response.Errors.Add("User already exists");
            }
        }

        public void ForgotPassword(AuthDTO login)
        {
            Login forgotLogin = authDAO.GetLoginByEmail(login.ForgotPassword.Request.Email);

            if(forgotLogin != null)
            {
                ResetPassword(forgotLogin);
                SendForgotEmail(login, forgotLogin);
            }
            else
            {
                login.ForgotPassword.Response.Errors.Add("Specified user doesn't exist");
            }
        }

        #region Private methods
        private void ResetPassword(Login login)
        {
            login.Password = Encrypt(Guid.NewGuid().ToString().Substring(0, 7));

            authDAO.Save(login);
        }

        private void SendForgotEmail(AuthDTO login, Login forgotLogin)
        {
            string to = forgotLogin.Email;
            string from = "webapiprueba@gmail.com";

            MailMessage message = new MailMessage(from, to)
            {
                Subject = "Password recovery",
                Body = $"Your new password is: {Decrypt(forgotLogin.Password)}"
            };

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                Credentials = new NetworkCredential("webapiprueba@gmail.com", "Aluxion123!")
            };

            try
            {
                smtp.Send(message);
            }
            catch (Exception ex)
            {
                login.ForgotPassword.Response.Errors.Add($"Error sending mail: {ex.Message}");
            }
        }

        #region Password encryption

        private static string Encrypt(string clearText)
        {
            string EncryptionKey = "ALUXION";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using MemoryStream ms = new MemoryStream();
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(clearBytes, 0, clearBytes.Length);
                    cs.Close();
                }
                clearText = Convert.ToBase64String(ms.ToArray());
            }
            return clearText;
        }

        private static string Decrypt(string cipherText)
        {
            string EncryptionKey = "ALUXION";
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

        #endregion
        #endregion

    }
}
