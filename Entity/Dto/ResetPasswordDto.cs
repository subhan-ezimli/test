using Entity.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Dto
{
    public class ResetPasswordDto:IDto 
    {
        public string OtpCode  { get; set; }
        public int UserId  { get; set; }
        public string Password  { get; set; }


    }
}
