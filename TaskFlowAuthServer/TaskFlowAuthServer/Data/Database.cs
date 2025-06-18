using MySql.Data.MySqlClient;

namespace TaskFlowAuthServer.Data
{
    public class Database
    {
        private readonly string connectionString = "server=localhost;user=root;password=852741;database=user_data;";
        public MySqlConnection conn;

        public Database()
        {
            conn = new MySqlConnection(connectionString);
            conn.Open();
        }

        public bool LoginUser(string email, string password, string token, string mac)
        {
            const string query = "SELECT COUNT(*) FROM users WHERE email = @Email AND password_hash = @Password";

            using var mySqlCommand = new MySqlCommand(query, conn);
            mySqlCommand.Parameters.AddWithValue("@Email", email);
            mySqlCommand.Parameters.AddWithValue("@Password", password);

            long count = (long)mySqlCommand.ExecuteScalar();

            if (count == 0)
            {
                return false;
            }

            if (!string.IsNullOrEmpty(token))
            {
                const string updateTokenQuery = "UPDATE users SET token = @Token, mac = @Mac WHERE email = @Email";
                using var updateCmd = new MySqlCommand(updateTokenQuery, conn);
                updateCmd.Parameters.AddWithValue("@Token", token);
                updateCmd.Parameters.AddWithValue("@Email", email);
                updateCmd.Parameters.AddWithValue("@Mac", mac);

                int rows = updateCmd.ExecuteNonQuery();

                return rows > 0;
            }
            return true;
        }

        public bool QuickLogin(string email, string token, string mac)
        {
            const string query = "SELECT 1 FROM users WHERE email = @Email AND token = @Token AND mac = @Mac";

            using var mySqlCommand = new MySqlCommand(query, conn);
            mySqlCommand.Parameters.AddWithValue("@Email", email);
            mySqlCommand.Parameters.AddWithValue("@Token", token);
            mySqlCommand.Parameters.AddWithValue("@Mac", mac);
            using var result = mySqlCommand.ExecuteReader();

            if (result.HasRows)
            {
                return true;
            }
            return false;
        }

        public bool AddUser(string email, string password)
        {
            bool exist = CheckUserExist(email);

            if (exist)
            {
                return false;
            }

            const string query = "INSERT INTO user_data.users (email, password_hash) VALUES (@Email, @Password)";

            using var mySqlCommand = new MySqlCommand(query, conn);
            mySqlCommand.Parameters.AddWithValue("@Email", email);
            mySqlCommand.Parameters.AddWithValue("@Password", password);

            int rowsAffected = mySqlCommand.ExecuteNonQuery();

            return rowsAffected > 0;
        }

        public bool CheckUserExist(string email)
        {
            const string query = "SELECT 1 FROM users WHERE email = @Email";
            using var mySqlCommand = new MySqlCommand(query, conn);
            mySqlCommand.Parameters.AddWithValue("@Email", email);
            using var result = mySqlCommand.ExecuteReader();

            if (result.HasRows)
            {
                return true;
            }
            return false;
        }
    }
}
