using backend.Interfaces.Services;
using backend.Models;
using backend.Models.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("add-user")]
        public async Task<IActionResult> AddUser([FromBody] RegistrationModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid registration data.");
            try
            {
                if (await _userService.UserExistsAsync(model.Email))
                    return Conflict("User already exists.");

                var success = await _userService.AddUserAsync(model);
                if (!success)
                {
                    return Conflict("Failed to add user.");
                }

                return Ok("User registered successfully.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error in AddUser: {e.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error.");
            }
        }
        
        [HttpGet("get-users")]
        public async Task<ActionResult<User>> GetUsers()
        {
            
            try
            {
                var users = await _userService.GetUsers();
                
                var response = new
                {
                    Message = "Users fetched successfully",
                    Data = users
                };
                return Ok(response);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error in AddUser: {e.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error.");
            }
        }
        
    }
}
