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

namespace ApiLayer.Controllers.api
{
    public class MemberController : ApiController
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 新增會員
        /// </summary>
        [HttpPost]
        public IHttpActionResult GetOtpAtVerifyPhone()
        {
            try
            {
                ResultResponse response;

                response = new ResultResponse { ErrorCode = accountAccessService.AddMember(addMemberDto) };
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
