using Dapper;
using Entity.Concrete;
using Entity.Dto;
using Entity.Enums;
using Microsoft.IdentityModel.Tokens;
using Repository.Abstract;
using Repository.Concrette.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Concrette
{
    public class UserRoleRepository : Repository<UserRole>, IUserRoleRepository
    {

        public UserRoleRepository (DatabaseSettings dbSettings)
              : base(dbSettings) { }

        public async Task<List<RoleDto>> GetUserRoles(int userId)
        {

            DbConnection.Open(); 

            var query = @$"Select [Roles].[Name] From [Roles]" +
                 $" join [UserRoles]  on [UserRoles].[RoleId] = [Roles].[Id]     " +
                 $"          Where [UserRoles].[UserId] = {userId} ";
            var result = DbConnection.Query<RoleDto>(query);
            return result.ToList();
        }
    }
    
}
