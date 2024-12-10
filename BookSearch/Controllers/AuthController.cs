using Microsoft.AspNetCore.Mvc;
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
        [HttpPost("login")]
        public async Task<ActionResult> LoginController([FromBody] Login request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.UserName) || string.IsNullOrWhiteSpace(request.Password))
                {
                    return BadRequest("Invalid username or password");
                }
                var token = await _authService.LoginUser(request.UserName, request.Password);
                return Ok(token);

            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
           

        }
        public IActionResult LogOut()
        {
            return Ok("Logged Out Successfully!!");
        }
    }
}
