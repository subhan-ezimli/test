using Core.Services;
using Core.Utilities.Results.Concrete;
using Dapper;
using Dapper.Contrib.Extensions;
using Entity.Concrete;
using Entity.Dto;
using Entity.Enums;
using Repository.Abstract;
using Repository.Concrette.Context;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Xml.Linq;

namespace Repository.Concrette
{
    public class UserRequestRepository : BaseRepository<UserRequest>, IUserRequestRepository
    {
        protected IDbConnection DbConnection  { get; private set; }
        private readonly DatabaseSettings _dbSettings;
 
        public UserRequestRepository(DatabaseSettings dbSettings) : base(dbSettings) 
        {
            _dbSettings = dbSettings;
            DbConnection = new DbContext().SetStrategy()
                .GetDbContext(_dbSettings.ConnectionString);
        }

        public async Task<List<GetActiveRequestDto>> GetActiveRequests ()
        {
            DbConnection.Open();
            var query = @$"Select [UserRequests].[Id],[UserRequests].[Word],[UserRequests].[Explanation],[Users].[Name]  From [UserRequests]" +
                $" join [Users]  on [UserRequests].[CreatedBy] = [Users].[Id]      " +
                $"          Where [UserRequests].[Status] = {(int)UserRequestStatus.active}  ORDER BY ID DESC";
            var result = DbConnection.Query<GetActiveRequestDto>(query);
            return result.ToList();
        }
        public async Task<bool> AcceptRequest (int requestId )
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                DbConnection.Open();
                var selectQuery = @$"Select us.[Word],us.[Explanation] From [UserRequests] us
                                 where us.[Id] = {requestId}";
                var resultSelect = DbConnection.Query<AcceptedWordDto>(selectQuery).FirstOrDefault();
                if (resultSelect == null)
                {
                    return false;
                }
                string insertQuery = @$"Insert Into [Dictionary] (Word,Explanation) Values(@Word,@Explanation)";
                int insertResult = await DbConnection.ExecuteAsync(insertQuery, new { Word = resultSelect.Word, Explanation = resultSelect.Explanation });
                if (insertResult == 0) 
                {
                    return false;
                }

                var queryUpdate = @$" update [UserRequests] set [Status] = {(int)UserRequestStatus.Confirmed}
                                  where [Id]= {requestId}";

                
                int updateResult = await DbConnection.ExecuteAsync(queryUpdate);

                if (updateResult == 0)
                {
                    return false;
                }
               
                scope.Complete();
                return true;
            }
        }
        public async Task<bool> RejectRequest (int requestId )
        {
            DbConnection.Open();
            var queryUpate = @$" update [UserRequests] set [Status] = {(int)UserRequestStatus.Rejected}
             where [Id]={requestId}";
              int resultUpdate = await DbConnection.ExecuteAsync(queryUpate);
          
             if (resultUpdate == 0)
             {
                 return false;
             }
            return true;
        }
       
    }
}
