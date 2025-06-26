using System.Text.Json.Serialization;

namespace TaskFlowAuthServer.Models
{
    public class QuickLoginModel
    {
        public string Email { get; set; }
        public string UserToken { get; set; }
        public string Mac { get; set; }
    }
}
