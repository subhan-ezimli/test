using Repository.Concrette.Context;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Design;
using Repository.Abstract;
using Entity.Concrete;
using Core.Utilities.Results.Concrete;
using Dapper;

namespace Repository.Concrette
{
    public class DictionaryRepository : Repository<Dictionary>, IDictionaryRepository
    {
        protected IDbConnection DbConnection { get; private set; }
        private readonly DatabaseSettings _dbSettings;

        public DictionaryRepository(DatabaseSettings dbSettings) : base(dbSettings)
        {
            _dbSettings = dbSettings;
            DbConnection = new DbContext().SetStrategy()
                .GetDbContext(_dbSettings.ConnectionString);
        }

        public Task<List<Dictionary>> Search(string key)
        {
            DbConnection.Open();
            var query = @$"Select * From [Dictionary] where [Word] Like '%@key%'";
            var result = DbConnection.Query<Dictionary>(query, new { key }).ToList();
            return Task.FromResult(result);
        }
    }
}
