using Entity.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Dto
{
    public class CheckConfirmedMessageDto:IDto 
    {
        public int UserId { get; set; }
        public string PhoneNumber { get; set; }
        public string Code { get; set; }
    }
}
