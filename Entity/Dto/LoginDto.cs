using Entity.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Dto
{
    public class LoginDto:IDto
    {
        public string Login  { get; set; }
        public string Password  { get; set; }
    }
}
