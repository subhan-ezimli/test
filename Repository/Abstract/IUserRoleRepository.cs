using Core.Utilities.Results.Abstract;
using Entity.Concrete;
using Entity.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Abstract
{
    public interface IUserRoleRepository : IRepository<UserRole>
    {
        Task<List<RoleDto>> GetUserRoles ( int userId );
      

    }
}
