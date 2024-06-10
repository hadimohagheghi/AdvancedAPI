using Data.Repositories;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebFramework.Api;


namespace MyApi.Controllers.v1
{

    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;



        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<ApiResult<List<User>>> Get(CancellationToken cancellationToken)
        {
            //HttpContext.RequestAborted =Output > CancellationToken
            var users = await _userRepository.TableNoTracking.ToListAsync(cancellationToken);

            return users;
            /* remove after implicit operator apiResult
             return new ApiResult<User>
             {
                 IsSuccess = true,
                 StatusCode = ApiResultStatusCode.Success,
                 Message = "عملیات با موفقیت انجام شد",
                 Data = users

             };*/

        }

        [HttpGet("{id:int}")]
        public async Task<ApiResult<User>> Get(int id, CancellationToken cancellationToken)
        {

            var user = await _userRepository.GetByIdAsync(cancellationToken, id);


            if (user == null)
            {
                return NotFound();
            }
            return user;
            /* remove after implicit operator apiResult
             return new ApiResult<User>
             {
                 IsSuccess = true,
                 StatusCode = ApiResultStatusCode.Success,
                 Message = "عملیات با موفقیت انجام شد",
                 Data = user

             };*/



        }

        [HttpPost]
        public async Task<ApiResult<User>> Create(User user, CancellationToken cancellationToken)
        {
             await _userRepository.AddAsync(user, cancellationToken);
             

            /*if (!ModelState.IsValid)
            {
                return BadRequest();
            }*/
            //Use ctor

            return Ok(user);
            //return new ApiResult(true, ApiResultStatusCode.Success); //or return new ApiResult(true, ApiResultStatusCode.Success,"عملیات با موفقیت انجام شد");
            /*return new ApiResult
            {
                IsSuccess = true,
                StatusCode = ApiResultStatusCode.Success,
                Message = "عملیات با موفقیت انجام شد",
            };*/

        }

        [HttpPut]
        public async Task<ApiResult> Update(int id, User user, CancellationToken cancellationToken)
        {
            var updateUser = await _userRepository.GetByIdAsync(cancellationToken, id);

            updateUser.UserName = user.UserName;
            updateUser.PasswordHash = user.PasswordHash;
            updateUser.FullName = user.FullName;
            updateUser.Age = user.Age;
            updateUser.Gender = user.Gender;
            updateUser.IsActive = user.IsActive;
            updateUser.LastLoginDate = user.LastLoginDate;

            await _userRepository.UpdateAsync(updateUser, cancellationToken);

            return Ok();
            //return new ApiResult(true, ApiResultStatusCode.Success);
            /*
            return new ApiResult
            {
                IsSuccess = true,
                StatusCode = ApiResultStatusCode.Success,
                Message = "عملیات با موفقیت انجام شد",
            };
            */
        }

        [HttpDelete]
        public async Task<ApiResult> Delete(int id, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(cancellationToken, id);
            await _userRepository.DeleteAsync(user, cancellationToken);

            return Ok();
            //return new ApiResult(true, ApiResultStatusCode.Success);
            /*return new ApiResult
            {
                IsSuccess = true,
                StatusCode = ApiResultStatusCode.Success,
                Message = "عملیات با موفقیت انجام شد",
            };*/
        }

    }
}
