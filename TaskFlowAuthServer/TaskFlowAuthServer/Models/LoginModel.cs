namespace TaskFlowAuthServer.Models
{
    public class LoginModel : BaseModel
    {
        public string? UserToken { get; set; }
        public string? Mac { get; set; }
    }
}
