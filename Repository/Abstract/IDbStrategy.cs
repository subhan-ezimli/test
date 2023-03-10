using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Abstract
{
    public interface IDbStrategy
    {
        IDbConnection GetConnection(string connectionString);
    }
}
