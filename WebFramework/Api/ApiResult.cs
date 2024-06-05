using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Common.Utilities;


namespace WebFramework.Api
{
    public class ApiResult
    {
        public bool IsSuccess { get; set; }
        public ApiResultStatusCode StatusCode { get; set; }
        public string Message { get; set; }


        public ApiResult(bool isSuccess, ApiResultStatusCode statusCode, string message=null)
        {
            IsSuccess = isSuccess;
            StatusCode = statusCode;
            Message = message??statusCode.ToDisplay(DisplayProperty.Name);
        }


        #region Implicit Operators
        public static implicit operator ApiResult(OkResult result)
        {
            return new ApiResult(true, ApiResultStatusCode.Success);
        }

        public static implicit operator ApiResult(BadRequestResult result)
        {
            return new ApiResult(false, ApiResultStatusCode.BadRequest);
        }

        public static implicit operator ApiResult(BadRequestObjectResult result)
        {
            var message = result.Value?.ToString();
            if (result.Value is SerializableError errors)
            {
                var errorMessages = errors.SelectMany(p => (string[])p.Value).Distinct();
                message = string.Join(" | ", errorMessages);
            }
            return new ApiResult(false, ApiResultStatusCode.BadRequest, message);
        }

        public static implicit operator ApiResult(ContentResult result)
        {
            return new ApiResult(true, ApiResultStatusCode.Success, result.Content);
        }

        public static implicit operator ApiResult(NotFoundResult result)
        {
            return new ApiResult(false, ApiResultStatusCode.NotFound);
        }
        #endregion
    }

    public class ApiResult<TData> : ApiResult
    {
        public TData Data { get; set; }



        #region Implicit Operators
        public static implicit operator ApiResult<TData>(TData data)
        {
           return new ApiResult<TData>(true, ApiResultStatusCode.Success, data);
            /* return new ApiResult<TData>
             {
                 IsSuccess = true,
                 StatusCode = ApiResultStatusCode.Success,
                 Data = data
             };
            */
        }
        



        #endregion

        public ApiResult(bool isSuccess, ApiResultStatusCode statusCode, TData data,string message = null) : base(isSuccess, statusCode, message)
        {
            Data = data;
        }
    }

    public enum ApiResultStatusCode
    {
        [Display(Name = "عملیات با موفقیت انجام شد")]
        Success = 0,

        [Display(Name = "خطایی در سرور رخ داده است")]
        ServerError = 1,

        [Display(Name = "پارامتر های ارسالی معتبر نیستند")]
        BadRequest = 2,

        [Display(Name = "یافت نشد")]
        NotFound = 3,

        [Display(Name = "لیست خالی است")]
        ListEmpty = 4,
    }



}
