using Entity.Abstract;
using Entity.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Dto
{
    public  class SendMessageDto:IDto 
    {
        public string PhoneNumber { get; set; }
        public MessageType MessageType { get; set; }
        public string Message  { get; set; }
        public string? Otpcode { get; set; }

    }
}
