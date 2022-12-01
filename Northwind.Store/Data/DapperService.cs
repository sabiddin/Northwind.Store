namespace Northwind.Store.Data
{
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Threading.Tasks;
    using Dapper;

    public class DapperService
    {
        private static readonly IDbConnection dbConnection = new SqlConnection(
            @"data source=CS310-L\SYED;initial catalog=Northwind;persist security info=True;Integrated Security=SSPI;");

        public static async Task<T> GetAsync<T>(string command, object parameters)
        {
            T result;

            result = (await dbConnection.QueryAsync<T>(command, parameters).ConfigureAwait(false)).
                FirstOrDefault();

            return result;
        }

        public static async Task<List<T>> GetAll<T>(string command, object parameters)
        {
            var result = new List<T>();

            result = (await dbConnection.QueryAsync<T>(command, parameters)).ToList();

            return result;
        }

        public static async Task<int> EditData(string command, object parameters)
        {
            int result;

            result = await dbConnection.ExecuteAsync(command, parameters);

            return result;
        }
    }
}