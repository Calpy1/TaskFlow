using System.Text.Json;
using System.IO;


namespace TaskFlow.Services
{
    public class CredentialsStorageService
    {
        public static void SaveUserCredentials(string email, string? token)
        {
            if (token == null)
            {
                return;
            }

            var userData = new { Email = email, Token = token };
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            string json = JsonSerializer.Serialize(userData, options);
            File.WriteAllText("user.dat", json);
        }
    }
}
