using Entity.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Concrete
{
    [Table("Users")]
    public class User :IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber  { get; set; } 
        public byte[] PasswordSalt { get; set; }
        public byte[] PasswordHash { get; set; }
        public DateTime CreatedDate  { get; set; }
        public DateTime? ModifiedDate  { get; set; }


    }
}
