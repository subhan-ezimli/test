using Core.Utilities.Results.Abstract;
using Entity.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IDictionaryService
    {
        Task<IDataResult<List<Dictionary>>> Search(string key);

    }
}
