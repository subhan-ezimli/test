using Business.Abstract;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete;
using Entity.Concrete;
using Repository.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrette
{
    public class DictionaryManager : IDictionaryService
    {
        private readonly IDictionaryRepository _dictionaryRepository;

        public DictionaryManager(IDictionaryRepository dictionaryRepository)
        {
            _dictionaryRepository = dictionaryRepository;
        }

        public async Task<IDataResult<List<Dictionary>>> Search(string key)
        {
            var result = await _dictionaryRepository.Search(key);
            return new SuccessDataResult<List<Dictionary>>(result,"Success");
        }
    }
}
