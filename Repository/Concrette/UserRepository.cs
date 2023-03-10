using Entity.Concrete;
using Entity.Dto;
using Repository.Abstract;
using Repository.Concrette.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 
    
namespace Repository.Concrette
{
    public class UserRepository : Repository<User>, IUserRepository
    { 

        public UserRepository (DatabaseSettings dbSettings)
              : base(dbSettings) { }
       
    }
}
