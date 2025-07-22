using Microsoft.AspNetCore.Mvc;
using WebApi.Models.Dtos.Login;
using WebApi.Services.Abstractions;
using WebApi.Utils;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _loginService;


        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }




        [HttpPost("jwt")]
        [JwtAuthenticate]
        public async Task<IActionResult> LoginJwt([FromBody] LoginRequest request)
        {
            var credentialsValid = await _loginService.ValidateCredentials(
                request.Username,
                request.Password
            );

            return Ok(true);
        }

    }
}
