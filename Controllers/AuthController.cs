﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebAPIFicheros.Models;
using WebAPIFicheros.Models.Dtos;
using WebAPIFicheros.Services;
using WebAPIFicheros.Services.Interfaces;

namespace WebAPIFicheros.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService = new AuthService();

        public AuthController()
        {
            
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login([FromBody] AuthDTO login)
        {
            return NoContent();
        }

        [HttpPost]
        [Route("Signup")]
        public IActionResult Signup([FromBody] AuthDTO login)
        {
            authService.Signup(login);

            return Ok();
        }

        [HttpPost]
        [Route("ForgotPassword")]
        public IActionResult ForgotPassword([FromBody] string email)
        {
            authService.ForgotPassword(email);

            return Ok();
        }

    }
}
