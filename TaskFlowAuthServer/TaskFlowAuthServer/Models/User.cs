namespace TaskFlowAuthServer.Models
{
    public class User
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string? UserToken { get; set; }
        public string? Mac { get; set; }
    }
}
