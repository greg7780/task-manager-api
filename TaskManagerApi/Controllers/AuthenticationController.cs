using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagerApi.DTOs;
using TaskManagerApi.Services;

namespace TaskManagerApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : Controller
    {
        private readonly AuthenticationService _authenticationService;

        public AuthenticationController(AuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDTO request)
        {
            var res = await _authenticationService.Login(request);
             
            if (string.IsNullOrWhiteSpace(res.Username)) return Unauthorized(new { message = res.Message });

            return Ok(res);
        }

        [AllowAnonymous]
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] string refreshToken)
        {
            var res = await _authenticationService.RefreshToken(refreshToken);
            if (string.IsNullOrWhiteSpace(res.Username)) return Unauthorized(new { message = res.Message });
            return Ok(res);
        }
    }
}
