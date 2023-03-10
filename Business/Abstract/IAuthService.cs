using Core.Utilities.Results.Abstract;
using Entity.Dto;
using Entity.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IAuthService
    {
        Task<IDataResult<GetMessageForRegisterDto>> RegisterFirst  (string PhoneNumber);
        Task<IDataResult<GetRegisterDto>> RegisterAsync(RegisterDto registerDto);

        Task<IDataResult<GetUserDto>> LoginAsync(LoginDto loginDto);
        Task<IResult> ChangePasswordASync(ChangePasswordDto changePasswordDto);


        Task<IDataResult<GetMessageDto>> ForgotPasswordAsync(string PhoneNumber);
        Task<IResult>  ResetPassword (ResetPasswordDto resetPasswordDto );


    }
}
