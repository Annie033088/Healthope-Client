using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiLayer.Models.Member.RequestMemberDto
{
    public class RequestVerifyOtpDto
    {
        /// <summary>
        /// 使用者輸入的 OTP code
        /// </summary>
        public string OtpCode {  get; set; }
    }
}