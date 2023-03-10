using Entity.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Dto
{
    public class UserRoleDto:IDto 
    {
        public int UserId { get; set; }
        public int RoleId  { get; set; }
    }
}
