using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPIFicheros.Models.Dtos
{
    public class AuthDTO
    {
        #region Login
        public LoginDTO Login { get; set; }
        public class LoginDTO
        {
            public RequestDTO Request { get; set; }
            public ResponseDTO Response { get; set; }
            public class RequestDTO
            {
                public string User { get; set; }
                public string Password { get; set; }
            }
            public class ResponseDTO : BaseDTO
            {
                public string Token { get; set; }
            }

            public LoginDTO()
            {
                Request = new RequestDTO();
                Response = new ResponseDTO();
            }
        }

        #endregion

        #region Signup
        public SignupDTO Signup { get; set; }
        public class SignupDTO
        {
            public RequestDTO Request { get; set; }
            public ResponseDTO Response { get; set; }
            public class RequestDTO
            {
                public string User { get; set; }
                public string Password { get; set; }
                public string Email { get; set; }
            }
            public class ResponseDTO : BaseDTO
            {
                
            }

            public SignupDTO()
            {
                Request = new RequestDTO();
                Response = new ResponseDTO();
            }
        }

        #endregion

        #region ForgotPassword
        public ForgotPasswordDTO ForgotPassword { get; set; }
        public class ForgotPasswordDTO
        {
            public RequestDTO Request { get; set; }
            public ResponseDTO Response { get; set; }
            public class RequestDTO
            {
                public string Email { get; set; }
            }
            public class ResponseDTO : BaseDTO
            {

            }

            public ForgotPasswordDTO()
            {
                Request = new RequestDTO();
                Response = new ResponseDTO();
            }
        }

        #endregion
    }
}
