using AutoMapper;
using Business.Abstract;
using Business.Constants;
using Core.Security;
using Core.Services;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete;
using Entity.Concrete;
using Entity.Dto;
using Entity.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Repository.Abstract;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Business.Concrette;

public class AuthManager : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;
    private readonly IUserRoleRepository _userRoleRepository;
    private readonly ISendMessageRepository _sendMessageRepository;
    public AuthManager(IUserRepository userRepository, IMapper mapper, IConfiguration configuration, IUserRoleRepository userRoleRepository, ISendMessageRepository sendMessageRepository)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _configuration = configuration;
        _userRoleRepository = userRoleRepository;
        _sendMessageRepository = sendMessageRepository;
    }

    private string _code = null;
    private RegisterDto _user;

    public Random random = new Random();
    public int RandomNumber(int min, int max)
    {
        return random.Next(min, max);
    }


    public async Task<IResult> ChangePasswordASync(ChangePasswordDto changePasswordDto)
    {
        try
        {
            var user = await _userRepository.GetFilter(x => x.Id == UserIdProvaider.GetUserId());

            if (!HashingHelper.VerifyPasswordHash(changePasswordDto.OldPassword, user.PasswordHash, user.PasswordSalt))
            {
                return new ErrorDataResult<ChangePasswordDto>(changePasswordDto, Messages.ErrorOldPasswordIsWrong);
            }
            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(changePasswordDto.NewPassword, out passwordHash, out passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.ModifiedDate = DateTime.Now;
            await _userRepository.UpdateAsync(user);
            return new SuccessResult(Messages.UserPasswordChanged);
        }

        catch (Exception exc)
        {
            return new ErrorDataResult<ChangePasswordDto>(exc.Message);
        }

    }

    public async Task<IDataResult<GetUserDto>> LoginAsync(LoginDto loginDto)
    {
        try
        {
            var user = await _userRepository.GetFilter(x => x.PhoneNumber == loginDto.Login);
            if (user == null)
            {
                return new ErrorDataResult<GetUserDto>(Messages.NotExsistUser);
            }
            if (!HashingHelper.VerifyPasswordHash(loginDto.Password, user.PasswordHash, user.PasswordSalt))
            {
                return new ErrorDataResult<GetUserDto>(Messages.FailLoginUser);
            }

            var authClaims = new List<Claim> {
                new Claim (ClaimTypes.Name, user.Id.ToString()),
                new Claim (JwtRegisteredClaimNames.Jti, Guid.NewGuid ().ToString ()),

                };

            var userRoles = await _userRoleRepository.GetUserRoles(user.Id);

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole.Name.ToString()));
            }

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));
            var token = new JwtSecurityToken(
             issuer: _configuration["JWT:ValidIssuer"],
             audience: _configuration["JWT:ValidAudience"],
             expires: DateTime.Now.AddYears(1),
             claims: authClaims,
             signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
         );
            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

            GetUserDto getUserDto = new GetUserDto();
            getUserDto.Name = user.Name;
            getUserDto.Surname = user.Surname;
            getUserDto.Token = accessToken;
            getUserDto.IsSuccess = true;


            return new SuccessDataResult<GetUserDto>(getUserDto, Messages.FoundUser);
        }
        catch (Exception exc)
        {
            return new ErrorDataResult<GetUserDto>(exc.Message);
        }
    }



    public async Task<IDataResult<GetRegisterDto>> RegisterAsync(RegisterDto registerDto)
    {
        try
        {
            var sendMessage  =_sendMessageRepository.GetFilter(x=>x.PhoneNumber==registerDto.PhoneNumber && x.Otpcode==registerDto.OptCode);
            var user = await _userRepository.GetFilter(x => x.PhoneNumber == registerDto.PhoneNumber);
            if (sendMessage == null||user!=null)
            {
                return new ErrorDataResult<GetRegisterDto>(Messages.AlreadyExsistPhoneNumber);
            }
            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(registerDto.Password, out passwordHash, out passwordSalt);

            var mappedUser = _mapper.Map<RegisterDto, User>(registerDto);
            mappedUser.PasswordHash = passwordHash;
            mappedUser.PasswordSalt = passwordSalt;
            mappedUser.CreatedDate = DateTime.Now;

            await _userRepository.CreateAsync(mappedUser);
             user= await _userRepository.GetFilter(x=>x.PhoneNumber==registerDto.PhoneNumber);
            var userRole = await _userRoleRepository.GetFilter(x => x.UserId == user.Id && x.RoleId == registerDto.RoleId);
            if (userRole == null)
            {
                var userRoleDto = new UserRoleDto()
                {
                    RoleId = registerDto.RoleId,
                    UserId = user.Id
                };
                var mappedUserRole = _mapper.Map<UserRoleDto, UserRole>(userRoleDto);
                await _userRoleRepository.CreateAsync(mappedUserRole);
            }
            GetRegisterDto getRegisterDto = new GetRegisterDto();
            getRegisterDto.UserId = user.Id;
            return new SuccessDataResult<GetRegisterDto>(getRegisterDto, Messages.EntityAdded);

        }
        catch (Exception exc)
        {
            return new ErrorDataResult<GetRegisterDto>(exc.Message);
        }
    }


    public async Task<IDataResult<GetMessageDto>> ForgotPasswordAsync(string PhoneNumber)
    {

        try
        {
            var user = await _userRepository.GetFilter(x => x.PhoneNumber == PhoneNumber);
            if (user != null)
            {
                SendMessageDto sendMessageDto = new SendMessageDto()
                {
                    Message = "Sizin OTP kodunuz:",
                    MessageType = MessageType.ForRecoveryPassword,
                    Otpcode = random.Next(1000, 9999).ToString(),
                    PhoneNumber = PhoneNumber
                };

                var mappedSendMessage = _mapper.Map<SendMessageDto, SendMessage>(sendMessageDto);
                mappedSendMessage.CreatedDate = DateTime.Now;
                await _sendMessageRepository.CreateAsync(mappedSendMessage);
                GetMessageDto getMessageDto = new GetMessageDto()
                {
                    Content = sendMessageDto.Message,
                    Otpcode = sendMessageDto.Otpcode,
                    UserId = user.Id

                };
                return new SuccessDataResult<GetMessageDto>(getMessageDto, Messages.EntityAdded);

            }
            return new ErrorDataResult<GetMessageDto>(Messages.ErrorEntityAdded);

        }
        catch (Exception ex)
        {

            return new ErrorDataResult<GetMessageDto>(ex.Message);
        }

    }


    public async Task<IResult> ResetPassword(ResetPasswordDto resetPasswordDto)
    {
        var user = await _userRepository.GetFilter(x => x.Id == resetPasswordDto.UserId);
        var sendMessage = await _sendMessageRepository.GetFilter(x => x.Otpcode == resetPasswordDto.OtpCode&& x.PhoneNumber==user.PhoneNumber);
        if (sendMessage != null)
        {
            byte[] passwordHash, passwordSalt;

            HashingHelper.CreatePasswordHash(resetPasswordDto.Password, out passwordHash, out passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.ModifiedDate = DateTime.Now;
            await _userRepository.UpdateAsync(user);
            return new SuccessResult(Messages.UserPasswordChanged);

        }
        return new ErrorResult(Messages.ErrorEntityUpdated);

    }

    public async Task<IDataResult<GetMessageForRegisterDto>> RegisterFirst (string PhoneNumber)
    {
        try 
        {
            var user = await _userRepository.GetFilter(x => x.PhoneNumber == PhoneNumber);
            if (user != null)
            {
                return new ErrorDataResult<GetMessageForRegisterDto>(Messages.AlreadyExsistPhoneNumber);
            }

            SendMessageDto sendMessageDto = new SendMessageDto()
            {
                Message = "Sizin OTP kodunuz:",
                MessageType = MessageType.ForRegister,
                Otpcode = random.Next(1000, 9999).ToString(),
                PhoneNumber = PhoneNumber
            };
            var mappedSendMessage = _mapper.Map<SendMessageDto, SendMessage>(sendMessageDto);
            mappedSendMessage.CreatedDate = DateTime.Now;
            await _sendMessageRepository.CreateAsync(mappedSendMessage);
            GetMessageForRegisterDto getMessageForRegisterDto = new GetMessageForRegisterDto()
            {
                Content = sendMessageDto.Message,
                Otpcode = sendMessageDto.Otpcode
            };
            return new SuccessDataResult<GetMessageForRegisterDto>(getMessageForRegisterDto, Messages.EntityAdded);
        }
        catch (Exception ex)
        {
            return new ErrorDataResult<GetMessageForRegisterDto>(ex.Message);
        }
    }
}
