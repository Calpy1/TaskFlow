using MySql.Data.MySqlClient;

namespace TaskFlowAuthServer.Data
{
    public class Database
    {
        private readonly string _connectionString = "server=localhost;user=root;password=852741;database=user_data;";

        public async Task<bool> LoginUserAsync(string email, string password)
        {
            const string query = "SELECT COUNT(*) FROM users WHERE email = @Email AND password_hash = @Password";

            await using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();

            await using var mySqlCommand = new MySqlCommand(query, conn);
            mySqlCommand.Parameters.AddWithValue("@Email", email);
            mySqlCommand.Parameters.AddWithValue("@Password", password);

            var result = await mySqlCommand.ExecuteScalarAsync();
            long count = Convert.ToInt64(result);

            return count > 0;
        }

        public async Task<bool> UpdateSessionAsync(string email, string token, string mac)
        {
            const string updateTokenQuery = "UPDATE users SET token = @Token, mac = @Mac WHERE email = @Email";

            await using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();

            await using var updateCmd = new MySqlCommand(updateTokenQuery, conn);
            updateCmd.Parameters.AddWithValue("@Email", email);
            updateCmd.Parameters.AddWithValue("@Token", token);
            updateCmd.Parameters.AddWithValue("@Mac", mac);

            int rows = await updateCmd.ExecuteNonQueryAsync();

            return rows > 0;
        }

        public async Task<bool> QuickLoginAsync(string email, string token, string mac)
        {
            const string query = "SELECT 1 FROM users WHERE email = @Email AND token = @Token AND mac = @Mac";

            await using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();

            await using var mySqlCommand = new MySqlCommand(query, conn);
            mySqlCommand.Parameters.AddWithValue("@Email", email);
            mySqlCommand.Parameters.AddWithValue("@Token", token);
            mySqlCommand.Parameters.AddWithValue("@Mac", mac);

            await using var result = await mySqlCommand.ExecuteReaderAsync();
            return await result.ReadAsync();
        }

        public async Task<bool> AddUserAsync(string email, string password)
        {
            bool exist = await CheckUserExistAsync(email);
            if (exist)
            {
                return false;
            }

            const string query = "INSERT INTO users (email, password_hash) VALUES (@Email, @Password)";

            await using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();

            await using var mySqlCommand = new MySqlCommand(query, conn);
            mySqlCommand.Parameters.AddWithValue("@Email", email);
            mySqlCommand.Parameters.AddWithValue("@Password", password);

            int rowsAffected = await mySqlCommand.ExecuteNonQueryAsync();

            return rowsAffected > 0;
        }

        public async Task<bool> CheckUserExistAsync(string email)
        {
            const string query = "SELECT 1 FROM users WHERE email = @Email";

            await using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();

            await using var mySqlCommand = new MySqlCommand(query, conn);
            mySqlCommand.Parameters.AddWithValue("@Email", email);

            await using var result = await mySqlCommand.ExecuteReaderAsync();

            return await result.ReadAsync();
        }
    }
}
