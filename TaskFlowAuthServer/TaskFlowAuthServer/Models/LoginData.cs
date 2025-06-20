namespace TaskFlowAuthServer.Models
{
    public class LoginData
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string? UserToken { get; set; }
        public string? Mac { get; set; }
    }
}
