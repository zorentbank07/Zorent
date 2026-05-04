
using Microsoft.AspNetCore.Mvc;
using Zorent.BLL.DTOs.Auth;
using Zorent.BLL.Interfaces;

namespace Zorent.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var result = await _authService.Register(dto);

            if (result != "Registration successful")
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("login")]

        public async Task<IActionResult> Login(LoginDto dto)
        {
            var result = await _authService.Login(dto);

            if (result == "Invalid username or password")
                return Unauthorized(result);

            return Ok(result);
        }

        [HttpGet("get-user")]
        public async Task<IActionResult> GetUser(string email)
        {
            var result = await _authService.GetUserByEmail(email);

            if (result == "User not found")
                return NotFound(result);

            return Ok(result);
        }

        [HttpGet("login-test")]
        public async Task<IActionResult> LoginGet(string username, string password)
        {
            var dto = new LoginDto
            {
                UserName = username,
                Password = password
            };

            var result = await _authService.Login(dto);

            if (result == "Invalid username or password")
                return Unauthorized(result);

            return Ok(result);
        }
    }
}
