using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiLayer.Models.ThirdParty;

namespace ApiLayer.Interface
{
    public interface IOtpService
    {
        /// <summary>
        /// 請求發送 OTP
        /// </summary>
        OtpSendResponse SendOtp(string phoneNumber, string expiry);

        /// <summary>
        /// 請求驗證 OTP
        /// </summary>
        bool VerifyOtp(string referenceId, string otpCode);
    }
}
