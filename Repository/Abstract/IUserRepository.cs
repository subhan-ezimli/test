using Dapper;
using Entity.Concrete;
using Repository.Concrette.Context;
using Repository.Concrette;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Abstract
{
    public interface IUserRepository : IRepository<User>
    {

    }
}
