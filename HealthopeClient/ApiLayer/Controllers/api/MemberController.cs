using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ApiLayer.Models.AccountAccess.RequestMemberDto;
using ApiLayer.Models;
using ApiLayer.Service;
using DomainLayer.Utility;
using NLog;
using ApiLayer.Interface;
using PersistentLayer.Models;
using ApiLayer.Models.Member.RequestMemberDto;
using ApiLayer.Models.Member.ReponseMemberDto;

namespace ApiLayer.Controllers.api
{
    public class MemberController : ApiController
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IMemberService memberService;

        public MemberController(IMemberService memberService)
        {
            this.memberService = memberService;
        }

        // TODO: 單元測試
        /// <summary>
        /// 驗證電話時，請求發送簡訊 OTP
        /// </summary>
        [HttpPost]
        public IHttpActionResult GetOtpAtVerifyPhone()
        {
            try
            {
               (ErrorCodeDefine errorCode, int remainingSecond) = memberService.GetOtpAtVerifyPhone();
                ResponseOtpRemainingTime responseDto = new ResponseOtpRemainingTime();
                responseDto.RemainingSecond = remainingSecond;
                ResultResponse<ResponseOtpRemainingTime> response = new ResultResponse<ResponseOtpRemainingTime> 
                { ErrorCode = errorCode, ApiDataObject = responseDto };
                return Ok(response);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                ResultResponse response = new ResultResponse() { ErrorCode = ErrorCodeDefine.ServerError };
                return Ok(response);
            }
        }

        // TODO: 單元測試
        /// <summary>
        /// 驗證電話時，請求發送簡訊 OTP
        /// </summary>
        [HttpPost]
        public IHttpActionResult VerifyPhone(RequestVerifyOtpDto verifyPhoneDto)
        {
            try
            {
                (ErrorCodeDefine errorCode, int remainingSecond) = memberService.VerifyPhone(verifyPhoneDto);
                ResponseOtpRemainingTime responseDto = new ResponseOtpRemainingTime();
                ResultResponse<ResponseOtpRemainingTime> response = new ResultResponse<ResponseOtpRemainingTime> 
                { ErrorCode = errorCode, ApiDataObject = responseDto };
                return Ok(response);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                ResultResponse response = new ResultResponse() { ErrorCode = ErrorCodeDefine.ServerError };
                return Ok(response);
            }
        }
    }
}
