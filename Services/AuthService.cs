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

        public void Signup(AuthDTO loginDTO)
        {
            var login = new Login()
            {
                User = loginDTO.User,
                Password = Encrypt(loginDTO.Password),
                Email = loginDTO.Email
            };

            authDAO.Add(login);
        }

        public void ForgotPassword(string email)
        {
            Login login = authDAO.GetLoginByEmail(email);

            if(login != null)
            {
                ResetPassword(login);
                SendForgotEmail(login);
            }
        }

        private void ResetPassword(Login login)
        {
            login.Password = Encrypt(Guid.NewGuid().ToString().Substring(0, 7));
        }

        private void SendForgotEmail(Login login)
        {
            string to = login.Email;
            string from = "aluxion@forgotpassword.com";

            MailMessage message = new MailMessage(from, to)
            {
                Subject = "Password recovery",
                Body = $"Your new password is: {login.Password}"
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
                throw new Exception($"Error al enviar mail: {ex.Message}");
            }
        }


        #region Token
        private static async Task<string> GetAuthorizeToken()
        {
            // Initialization.  
            string responseObj = string.Empty;

            // Posting.  
            using (var client = new HttpClient())
            {
                // Setting Base address.  
                client.BaseAddress = new Uri("http://localhost:3097/");

                // Setting content type.  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Initialization.  
                HttpResponseMessage response = new HttpResponseMessage();
                List<KeyValuePair<string, string>> allIputParams = new List<KeyValuePair<string, string>>();

                // Convert Request Params to Key Value Pair.  

                // URL Request parameters.  
                HttpContent requestParams = new FormUrlEncodedContent(allIputParams);

                // HTTP POST  
                response = await client.PostAsync("Token", requestParams).ConfigureAwait(false);

                // Verification  
                if (response.IsSuccessStatusCode)
                {
                    // Reading Response.  

                }
            }

            return responseObj;
        }

        private static async Task<string> GetInfo(string authorizeToken)
        {
            // Initialization.  
            string responseObj = string.Empty;

            // HTTP GET.  
            using (var client = new HttpClient())
            {
                // Initialization  
                string authorization = authorizeToken;

                // Setting Authorization.  
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                // Setting Base address.  
                client.BaseAddress = new Uri("https://localhost:44334/");

                // Setting content type.  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Initialization.  
                HttpResponseMessage response = new HttpResponseMessage();

                // HTTP GET  
                response = await client.GetAsync("api/WebApi").ConfigureAwait(false);

                // Verification  
                if (response.IsSuccessStatusCode)
                {
                    // Reading Response.  
                }
            }

            return responseObj;
        }
        #endregion

        #region Password encryption

        private string Encrypt(string clearText)
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

        private string Decrypt(string cipherText)
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
    }
}
