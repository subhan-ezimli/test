using Business.Abstract;
using Core.Utilities.Results.Abstract;
using Entity.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService= authService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDto  registerDto )
        {
           var data =  await _authService.RegisterAsync(registerDto);
            return Ok(data);
        }

        

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword (ResetPasswordDto resetPasswordDto )
        {
            var data = await _authService.ResetPassword(resetPasswordDto);
            return Ok(data);
        }



        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto changePasswordDto)
        {
            var data = await _authService.ChangePasswordASync(changePasswordDto);
            return Ok(data);
        }

        [HttpPost("ForgotMyPassword")]
        public async Task<IActionResult> ForgotMyPassword(string PhoneNumber )
        {
            var data = await _authService.ForgotPasswordAsync(PhoneNumber);
            return Ok(data);
        }

        [HttpPost("RegisterFirst")]
        public async Task<IActionResult> RegisterFirst(string PhoneNumber)
        {
            var data = await _authService.RegisterFirst(PhoneNumber);
            return Ok(data);
        }


        [HttpPost("Login")]
        public async Task<IActionResult> Login (LoginDto loginDto )
        {
            var data = await _authService.LoginAsync(loginDto);
            return Ok(data);
        }
       
    }
}
