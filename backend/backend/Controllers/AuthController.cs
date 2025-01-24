using backend.Extensions;
using backend.Interfaces.Services;
using backend.Models.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/chat")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserService _userService;

        public AuthController(IAuthenticationService authenticationService,
            IUserService userService)
        {
            _authenticationService = authenticationService;
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthenticationsToken>> Login([FromBody] LoginModel loginModel)
        {
            try
            {
                var user = await _userService.GetUserByEmailAsync(loginModel.Email);
                if (user == null || !PasswordHasher.VerifyHash(loginModel.Password, user.HashedPassword))
                {
                    return Unauthorized("Invalid credentials");
                }
                var token = await _authenticationService.Login(user);
                if (token == null)
                {
                    return StatusCode(500, "Failed to generate authentication token.");
                }
                return Ok(token);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }
        
        
    }
}
