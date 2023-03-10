using Entity.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Dto
{
    public class ChangePasswordDto : IDto
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}

