using Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Concrete
{
    [Table("UserRoles")]
    public class UserRole  : IEntity
    {
        public int Id  { get; set; }
        public int UserId { get; set; }
        public int  RoleId  { get; set; }
    }
}
