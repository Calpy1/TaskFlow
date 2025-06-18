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
        public IActionResult Register([FromBody] User user)
        {
            bool valid = _db.AddUser(user.Email, user.Password);

            if (valid)
            {
                return Created("", "User registered successfully");
            }
            return Conflict("Registration failed");
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] User user)
        {
            bool valid = _db.LoginUser(user.Email, user.Password, user.UserToken, user.Mac);

            if (valid)
            {
                return Ok("Successfully logged in");
            }
            return Unauthorized("Invalid email or password");
        }

        [HttpPost("quicklogin")]
        public IActionResult QuickLogin([FromBody] User user)
        {
            bool valid = _db.QuickLogin(user.Email, user.UserToken, user.Mac);

            if (valid)
            {
                return Ok("Successfully logged in");
            }
            return Unauthorized("Invalid email or password");
        }
    }
}
