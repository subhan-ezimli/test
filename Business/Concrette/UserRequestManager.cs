using AutoMapper;
using Business.Abstract;
using Business.Constants;
using Core.Services;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete;
using Entity.Dto;
using Entity.Concrete;
using Entity.Enums;
using Repository.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;

namespace Business.Concrette 
{
    public class UserRequestManager : IUserRequestService
    {
        private readonly IMapper _mapper;
        private readonly IUserRequestRepository _userRequestRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        public UserRequestManager(IMapper mapper, IUserRequestRepository userRequestRepository,IUserRoleRepository userRoleRepository)
        {
            _mapper = mapper;
            _userRequestRepository = userRequestRepository;
            _userRoleRepository = userRoleRepository;
        }

        

        public async Task<IDataResult<RequestDto>> RequestAdd (RequestDto requestDto )
        {
            try
            { 
                var mappedentity = _mapper.Map<RequestDto, UserRequest>(requestDto);
                mappedentity.Status = UserRequestStatus.active;

                await _userRequestRepository.CreateAsync(mappedentity);
                return new SuccessDataResult<RequestDto>(requestDto, Messages.EntityAdded);
            }
            catch (Exception ex)
            {

                return new SuccessDataResult<RequestDto>(ex.Message);

            }
          

        }

        public async Task<IDataResult<List<GetActiveRequestDto>>> GetActiveRequests ()
        {

            try
            {  var userRole = await _userRoleRepository.GetUserRoles(UserIdProvaider.GetUserId());
                foreach (var item in userRole)
                {
                    if (item.Name=="Admin")
                    {
                        var result = await _userRequestRepository.GetActiveRequests();
                        return new SuccessDataResult<List<GetActiveRequestDto>>(result, Messages.EntityAdded);
                    }
                }
                
                return new ErrorDataResult<List<GetActiveRequestDto>>(Messages.ErrorInUserRole);
            }
            catch (Exception ex)
            {

                return new ErrorDataResult<List<GetActiveRequestDto>>(ex.Message);

            }


        }
        public async Task<IResult> AcceptRequest  (int requestId )
        {
            try
            {
                var userRole = await _userRoleRepository.GetUserRoles(UserIdProvaider.GetUserId());
                foreach (var item in userRole)
                {
                    if (item.Name == "Admin")
                    {
                        var result = await _userRequestRepository.AcceptRequest(requestId );
                        if (result)
                        {
                            return new SuccessResult("Success");
                        }

                        return new ErrorResult("Fault");
                    }
                }
                
                return new ErrorResult(Messages.ErrorInUserRole);
            }
            catch (Exception ex )
            {

                return new ErrorResult(ex.Message);
            }
            

        }
        public async Task<IResult> RejectRequest (int requestId )
        {
            try
            {
                var userRole = await _userRoleRepository.GetUserRoles(UserIdProvaider.GetUserId());
                foreach (var item in userRole)
                {
                    if (item.Name == "Admin")
                    {

                        var result = await _userRequestRepository.RejectRequest(requestId);
                        if (result)
                        {
                            return new SuccessResult(Messages.EntityAdded);
                        }
                        return new ErrorResult( Messages.ErrorInUserRole);
                    }
                }
                
               return new ErrorResult(Messages.ErrorInUserRole);
            }
            catch (Exception ex)
            {

                return new ErrorResult(ex.Message);
            }
           
        }

        
    }
}
