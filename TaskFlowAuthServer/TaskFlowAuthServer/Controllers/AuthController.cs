using Microsoft.AspNetCore.Mvc;
using TaskFlowAuthServer.Models;
using TaskFlowAuthServer.Data;

namespace TaskFlowAuthServer.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly Database _db;

        public AuthController()
        {
            _db = new Database();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel registerModel)
        {
            bool valid = await _db.AddUserAsync(registerModel.Email, registerModel.Password);

            if (valid)
            {
                return Created("", "User registered successfully");
            }
            return Conflict("Registration failed");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            bool valid = await _db.LoginUserAsync(loginModel.Email, loginModel.Password);

            if (!string.IsNullOrEmpty(loginModel.Email) && !string.IsNullOrEmpty(loginModel.UserToken) && !string.IsNullOrEmpty(loginModel.Mac))
            {
                await _db.UpdateSessionAsync(loginModel.Email, loginModel.UserToken, loginModel.Mac);
            }

            if (valid)
            {
                return Ok("Successfully logged in");
            }
            return Unauthorized("Invalid email or password");
        }

        [HttpPost("quicklogin")]
        public async Task<IActionResult> QuickLogin([FromBody] QuickLoginModel quickLoginModel)
        {
            if (await _db.CheckUserExistAsync(quickLoginModel.Email))
            {
                await _db.UpdateSessionAsync(quickLoginModel.Email, quickLoginModel.UserToken, quickLoginModel.Mac);
            }

            bool valid = await _db.QuickLoginAsync(quickLoginModel.Email, quickLoginModel.UserToken, quickLoginModel.Mac);

            if (valid)
            {
                return Ok("Successfully logged in");
            }
            return Unauthorized("Invalid email or password");
        }
    }
}
