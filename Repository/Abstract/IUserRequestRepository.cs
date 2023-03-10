using Entity.Concrete;
using Entity.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Abstract
{
    public interface IUserRequestRepository  : IBaseRepository<UserRequest>
    {
        Task<List<GetActiveRequestDto>> GetActiveRequests ();
        Task<bool> AcceptRequest (int requestId );
        Task<bool> RejectRequest (int requestId );
     
    }
}
