using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiLayer.Models;
using ApiLayer.Models.Member.RequestMemberDto;

namespace ApiLayer.Interface
{
    public interface IMemberService
    {
        /// <summary>
        /// 請求第三方發送 OTP，並儲存
        /// </summary>
        (ErrorCodeDefine, int ttl) GetOtpAtVerifyPhone();

        /// <summary>
        /// 驗證手機號碼
        /// </summary>
        (ErrorCodeDefine, int ttl) VerifyPhone(RequestVerifyOtpDto verifyPhoneDto);
    }
}
