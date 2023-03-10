using Entity.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Dto
{
  public class GetUserDto:IDto
    {   
        public string Token  { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public bool IsSuccess  { get; set; }
    }         
}
