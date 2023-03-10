using Business.Abstract;
using Entity.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRequestController : ControllerBase
    {
        private readonly IUserRequestService _userRequestService ;
        public UserRequestController(IUserRequestService userRequestService )
        {
            _userRequestService = userRequestService;
        }

        [HttpPost("RequestAdd")]
        public async Task<IActionResult> RequestAdd (RequestDto applyDto)
        {
            var data = await _userRequestService.RequestAdd(applyDto);
            return Ok(data);
        }

        [HttpGet("GetActiveRequests")]
        public async Task<IActionResult> GetActiveRequests ()
        {
            var data = await _userRequestService.GetActiveRequests();
            return Ok(data);
        }
        
        [HttpPost("AcceptRequest")]
        public async Task<IActionResult> AcceptRequest (int requestId )
        {
            var data = await _userRequestService.AcceptRequest(requestId );
            return Ok(data);
        }

        [HttpPost("RejectRequest ")]
        public async Task<IActionResult> RejectRequest (int requestId )
        {
            var data = await _userRequestService.RejectRequest(requestId);
            return Ok(data);
        }
    }
}
