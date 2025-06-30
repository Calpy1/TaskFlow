using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Relational;
using System.Data;
using System.Diagnostics;

namespace TaskFlowAuthServer.Models
{
    public class LoginModel : BaseModel
    {
        public string? UserToken { get; set; }
        public string? Mac { get; set; }

        public async Task<bool> CheckLoginCredentials(LoginModel loginModel)
        {
            try
            {
                string query = "SELECT COUNT(*) FROM users WHERE email = @Email AND password_hash = @Password";

                var parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@Email", loginModel.Email),
                    new MySqlParameter("@Password", loginModel.Password),
                };

                var result = await GetScalarValueAsync(query, parameters);
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<bool> CheckQuickLoginCredentials(QuickLoginModel quickLoginModel)
        {
            try
            {
                string query = "SELECT 1 FROM users WHERE email = @Email AND token = @Token AND mac = @Mac";

                var parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@Email", quickLoginModel.Email),
                    new MySqlParameter("@Token", quickLoginModel.UserToken),
                    new MySqlParameter("@Mac", quickLoginModel.Mac)
                };

                var result = await GetScalarValueAsync(query, parameters);
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<bool> UpdateSessionAsync(LoginModel loginModel)
        {
            try
            {
                string query = "UPDATE users SET token = @Token, mac = @Mac WHERE email = @Email";

                var parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@Token", loginModel.UserToken),
                    new MySqlParameter("@Mac", loginModel.Mac),
                    new MySqlParameter("@Email", loginModel.Email),
                };

                var result = await ExecuteNonQueryAsync(query, parameters);
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<int?> CheckCompanyExistAsync(RegisterModel registerModel)
        {
            try
            {
                string query = "SELECT id FROM companies_data.companies WHERE slug = @CompanySlug";

                var parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@CompanySlug", registerModel.CompanySlug),
                };

                DataTable dataTable = await ExecuteQueryAsync(query, parameters);
                
                if (dataTable.Rows.Count > 0)
                {
                    return Convert.ToInt32(dataTable.Rows[0]["id"]);
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<bool> TryAddUserAsync(RegisterModel registerModel)
        {
            try
            {
                int? companyId = await CheckCompanyExistAsync(registerModel);

                if (companyId != null)
                {
                    string query = "INSERT INTO users (email, password_hash, company_id) VALUES (@Email, @Password, @CompanyId)";

                    var parameters = new MySqlParameter[]
                    {
                        new MySqlParameter("@Email", registerModel.Email),
                        new MySqlParameter("@Password", registerModel.Password),
                        new MySqlParameter("@CompanyId", companyId),
                    };

                    var result = await ExecuteNonQueryAsync(query, parameters);
                    return result;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
