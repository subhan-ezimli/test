using Repository.Abstract;
using System.Data;
using System.Data.SqlClient;

namespace Repository.Concrette.Context;

public class SqlServerStrategy : IDbStrategy
{
    public IDbConnection GetConnection(string connectionString)
    {
        return new SqlConnection(connectionString);
    }
}
