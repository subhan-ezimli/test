using Entity.Abstract;
using Entity.Concrete;
using Entity.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Entity.Dto
{
    public class RegisterDto:IDto
    {
        public string OptCode  { get; set; } 
        public string Name { get; set; }
        public string Surname  { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public int RoleId  { get; set; }


    }
}
