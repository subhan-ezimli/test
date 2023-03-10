using Entity.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Dto
{
    public class AcceptedWordDto:IDto
    {
        public string Word { get; set; }
        public string Explanation { get; set; }
    }
}
