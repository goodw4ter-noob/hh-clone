using Dapper;
using Npgsql;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace hh_clone.DAL
{
    public class DbHelper
    {
        public static string connString = "User ID=postgres;Password=password;Host=localhost;Port=5432;Database=test";

        public static async Task<int> ExecuteScalarAsync(string sql, object model)
        {
            using (var connection = new NpgsqlConnection(DbHelper.connString))
            {
                await connection.OpenAsync();

                return await connection.ExecuteAsync(sql, model);
            }
        }

        public static async Task<IEnumerable<T>> QueryAsync<T>(string sql, object model)
        {
			using (var connection = new NpgsqlConnection(DbHelper.connString)) 
            {
                await connection.OpenAsync();

                return await connection.QueryAsync<T>(sql, model);
            }

		}
    }
}
