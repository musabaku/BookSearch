﻿using Microsoft.AspNetCore.Mvc;
using BookSearch.Services;
using BookSearch.Model;
using Microsoft.AspNetCore.Identity.Data;
namespace BookSearch.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpPost("register")]
        public async Task<ActionResult> RegisterController([FromBody] Register request)
        {
            if (string.IsNullOrWhiteSpace(request.UserName) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest("Provide username and password");
            }

           var result = await _authService.RegisterUser(request.UserName, request.Password);
            return result? Ok("User created"): Ok("User alerady exists, login!");

        }
    }
}