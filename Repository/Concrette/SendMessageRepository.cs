using Dapper;
using Entity.Concrete;
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
    public class SendMessageRepository : Repository<SendMessage>, ISendMessageRepository
    {

        public SendMessageRepository(DatabaseSettings dbSettings)
              : base(dbSettings) { }

    }

}
