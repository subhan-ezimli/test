using Entity.Abstract;
using Entity.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Concrete 
{
    [Table("SendMessages")]
    public class SendMessage :IEntity
    {
        public int Id  { get; set; }
        public string PhoneNumber  { get; set; }
        public MessageType MessageType { get; set; }
        public string Message   { get; set; }
        public string?  Otpcode   { get; set; }
        public DateTime  CreatedDate   { get; set; }
    }
}
