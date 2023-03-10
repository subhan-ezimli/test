﻿using Entity.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Dto
{
    public class GetActiveRequestDto :IDto 
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Word { get; set; }
        public string Explanation  { get; set; }

    }
}
