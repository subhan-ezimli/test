using Entity.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Abstract
{
    public interface IDictionaryRepository : IRepository<Dictionary>
    {
        Task<List<Dictionary>> Search(string key);
    }
}
