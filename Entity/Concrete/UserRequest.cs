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
    [Table("UserRequests")]
    public class UserRequest :BaseEntity
    {
        public string Word { get; set; }
        public string Explanation  { get; set; }
        public UserRequestStatus  Status { get; set; }
        public DateTime? ModifiedDate  { get; set; }
    }
}
