using Entity.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Dto
{
    public class GetMessageDto:IDto 
    {
        public string Content { get; set; }
        public string? Otpcode { get; set; }
        public int  UserId  { get; set; }
    }
}
