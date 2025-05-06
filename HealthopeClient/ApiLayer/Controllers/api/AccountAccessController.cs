using System;
using System.Web.Http;
using ApiLayer.Filters;
using ApiLayer.Interface;
using ApiLayer.Models;
using ApiLayer.Models.AccountAccess.RequestMemberDto;
using DomainLayer.Utility;
using NLog;

namespace ApiLayer.Controllers.api
{
    public class AccountAccessController : ApiController
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IAccountAccessService accountAccessService;

        public AccountAccessController(IAccountAccessService accountAccessService)
        {
            this.accountAccessService = accountAccessService;
        }

        /// <summary>
        /// 新增會員
        /// </summary>
        [HttpPost]
        public IHttpActionResult AddMember([FromBody] RequestAddMemberDto addMemberDto)
        {
            try
            {
                ResultResponse response;
                // 格式驗證
                FormatValidation formatValidation = new FormatValidation();

                if (!formatValidation.ValidAccount(addMemberDto.Account) ||
                    !formatValidation.ValidPwd(addMemberDto.Pwd) ||
                    !formatValidation.ValidEmail(addMemberDto.Email) ||
                    !formatValidation.ValidPhone(addMemberDto.Phone) ||
                    addMemberDto.Account == addMemberDto.Pwd) // 帳號密碼不可相同
                {
                    response = new ResultResponse { ErrorCode = ErrorCodeDefine.InvalidFormatOrEntry };
                    return Ok(response);
                }

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

        /// <summary>
        /// 會員登出
        /// </summary>
        [HttpPost]
        [VeriyLoginFilter]
        public IHttpActionResult MemberLogout()
        {
            try
            {
                if (accountAccessService.MemberLogout())
                {
                    ResultResponse response = new ResultResponse { ErrorCode = ErrorCodeDefine.Success };
                    return Ok(response);
                }
                else
                {
                    ResultResponse response = new ResultResponse() { ErrorCode = ErrorCodeDefine.ServerError };
                    return Ok(response);
                }
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
