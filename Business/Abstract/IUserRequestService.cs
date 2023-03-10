using Core.Utilities.Results.Abstract;
using Entity.Concrete;
using Entity.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IUserRequestService 
    {
        Task<IDataResult<RequestDto>> RequestAdd  (RequestDto requestDto );
        Task<IDataResult<List<GetActiveRequestDto>>> GetActiveRequests ();
        Task<IResult> AcceptRequest (int requestId );
        Task<IResult> RejectRequest (int requestId );
    }
}
