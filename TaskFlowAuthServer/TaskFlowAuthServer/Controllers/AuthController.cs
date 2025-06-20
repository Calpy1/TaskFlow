using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using TaskFlowAuthServer.Models;
using TaskFlowAuthServer.Data;
using System.Text;

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
        public IActionResult Register([FromBody] RegisterData registerData)
        {
            bool valid = _db.AddUser(registerData.Email, registerData.Password);

            if (valid)
            {
                return Created("", "User registered successfully");
            }
            return Conflict("Registration failed");
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginData loginData)
        {
            bool valid = _db.LoginUser(loginData.Email, loginData.Password);

            if (!string.IsNullOrEmpty(loginData.Email) && !string.IsNullOrEmpty(loginData.UserToken) && !string.IsNullOrEmpty(loginData.Mac))
            {
                _db.UpdateSession(loginData.Email, loginData.UserToken, loginData.Mac);
            }

            if (valid)
            {
                return Ok("Successfully logged in");
            }
            return Unauthorized("Invalid email or password");
        }

        [HttpPost("quicklogin")]
        public IActionResult QuickLogin([FromBody] QuickLoginData quickLoginData)
        {
            if (_db.CheckUserExist(quickLoginData.Email))
            {
                _db.UpdateSession(quickLoginData.Email, quickLoginData.UserToken, quickLoginData.Mac);
            }

            bool valid = _db.QuickLogin(quickLoginData.Email, quickLoginData.UserToken, quickLoginData.Mac);

            if (valid)
            {
                return Ok("Successfully logged in");
            }
            return Unauthorized("Invalid email or password");
        }
    }
}
