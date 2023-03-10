using Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Entity.Concrete   
{
    [Table("Dictionary")]
    public class Dictionary:IEntity      
    {  
        public string Word { get; set; }
        public string Explanation { get; set; }
    }
}
