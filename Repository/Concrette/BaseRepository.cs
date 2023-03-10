using Core.Services;
using Dapper;
using Dapper.Contrib.Extensions;
using Entity.Abstract;
using Repository.Abstract;
using Repository.Concrette.Context;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Concrette
{
   
    public class BaseRepository<TEntity>  where TEntity : BaseEntity, new()
    {
        protected IDbConnection DbConnection { get; private set; }
        private readonly DatabaseSettings _dbSettings;

        public BaseRepository(DatabaseSettings dbSettings)
        {
            _dbSettings = dbSettings;
            DbConnection = new DbContext().SetStrategy()
                .GetDbContext(_dbSettings.ConnectionString);
        }

        public async Task<bool> CreateAsync(TEntity entity)
        {
            DbConnection.Open();
            try
            {   
             entity.CreatedDate = DateTime.Now;
             entity.CreatedBy = UserIdProvaider.GetUserId();
                var inserted = await DbConnection
                .InsertAsync<TEntity>(entity);
               
                return inserted == 0;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally { DbConnection.Close(); }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            DbConnection.Open();

            try
            {
               
                var entity = await DbConnection
                    .GetAsync<TEntity>(id);

                if (entity == null)
                    return false;
                 
                return await DbConnection
                    .DeleteAsync<TEntity>(entity);
            }
            finally { DbConnection.Close(); }
        }

        public async Task<List<TEntity>> FindAllAsync()

        {
            DbConnection.Open();

            try
            {
                var results = await DbConnection
                    .GetAllAsync<TEntity>();

                return results
                    .ToList();
            }
            finally { DbConnection.Close(); }
        }

        public async Task<TEntity> FindByIdAsync(int id)
        {
            DbConnection.Open();

            try
            {
                return await DbConnection
                    .GetAsync<TEntity>(id);
            }
            finally { DbConnection.Close(); }
        }

        public async Task<bool> UpdateAsync(TEntity entity)
        {
            DbConnection.Open();

            try
            {
                return await DbConnection
                    .UpdateAsync<TEntity>(entity);
            }
            finally { DbConnection.Close(); }
        }
        public async Task<TEntity> GetFilter(Expression<Func<TEntity, bool>> filter)
        {
            DbConnection.Open();
            try
            {
                var data = await DbConnection.GetAllAsync<TEntity>();
                var results = data.AsQueryable().SingleOrDefault(filter);
                return results;
            }
            finally { DbConnection.Close(); }
        }
        public async Task<List<TEntity>> GetFilterAll(Expression<Func<TEntity, bool>> filter)
        {
            DbConnection.Open();

            try
            {
                var data = await DbConnection.GetAllAsync<TEntity>();
                var results = data.AsQueryable().Where(filter).ToList();
                return results;
            }
            finally { DbConnection.Close(); }
        }
        public async Task<int> GetStoredProcedure(string storedProcedure, DynamicParameters dynamicParameters)
        {
            DbConnection.Open();
            try
            {
                var results = await DbConnection.ExecuteAsync(storedProcedure, dynamicParameters, commandType: CommandType.StoredProcedure);
                return results;
            }
            finally { DbConnection.Close(); }
        }
        public async Task<List<TEntity>> GetQueryAll(string query)
        {
            DbConnection.Open();

            try
            {
                var data = await DbConnection.QueryAsync<TEntity>(query);
                return data.ToList();
            }
            finally { DbConnection.Close(); }
        }
    }
}
