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
        private readonly LoginModel _loginModel = new LoginModel();

        public AuthController()
        {
            _db = new Database();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel registerModel) // login, email, companySlug
        {
            bool valid = await _loginModel.TryAddUserAsync(registerModel);

            if (valid)
            {
                return Created("", "User registered successfully");
            }
            return Conflict("Registration failed");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            bool valid = await _loginModel.CheckLoginCredentials(loginModel); // email, password

            if (!string.IsNullOrEmpty(loginModel.Email) && !string.IsNullOrEmpty(loginModel.UserToken) && !string.IsNullOrEmpty(loginModel.Mac))
            {
                await _loginModel.UpdateSessionAsync(loginModel);
            }

            if (valid)
            {
                return Ok("Successfully logged in");
            }
            return Unauthorized("Invalid email or password");
        }

        [HttpPost("quicklogin")]
        public async Task<IActionResult> QuickLogin([FromBody] QuickLoginModel quickLoginModel) // email, token, mac 
        {
            bool valid = await _loginModel.CheckQuickLoginCredentials(quickLoginModel);

            if (valid)
            {
                return Ok("Successfully logged in");
            }
            return Unauthorized("Invalid email or password");
        }
    }
}
