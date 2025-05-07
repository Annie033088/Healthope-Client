using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using ApiLayer.Interface;
using ApiLayer.Models;
using ApiLayer.Models.Member;
using ApiLayer.Models.Member.RequestMemberDto;
using ApiLayer.Models.ThirdParty;
using PersistentLayer.Interface;
using PersistentLayer.Repository;
using Pipelines.Sockets.Unofficial.Arenas;

namespace ApiLayer.Service
{
    public class MemberService : IMemberService
    {
        private readonly string memberSessionKey = "MemberSessionKey";
        private readonly IMemberRepository memberRepository;
        private readonly ISessionService sessionService;
        private readonly IOtpService otpService;
        private readonly IRedisService redisService;

        public MemberService(IMemberRepository memberRepository, ISessionService sessionService,
            IOtpService otpService, IRedisService redisService)
        {
            this.memberRepository = memberRepository;
            this.sessionService = sessionService;
            this.otpService = otpService;
            this.redisService = redisService;
        }

        // TODO: 單元測試
        public (ErrorCodeDefine, int ttl) GetOtpAtVerifyPhone()
        {
            try
            {
                int otpExpiry = 180;
                MemberSession memberSession = sessionService.GetSession<MemberSession>(memberSessionKey);
                (int errorCodeNumber, int phone) = memberRepository.GetPhoneAtVerifyPhone(memberSession.MemberId);

                // 沒有拿到使用者電話
                if (phone == -1) return (ErrorCodeDefine.ServerError, 0);
                // 如果沒有被定義在 enum 裡
                if (!Enum.IsDefined(typeof(ErrorCodeDefine), errorCodeNumber)) return (ErrorCodeDefine.ServerError, 0);

                ErrorCodeDefine errorCode = (ErrorCodeDefine)errorCodeNumber;
                // 號碼已驗證 或其他狀況
                if (errorCode != ErrorCodeDefine.Success) return (errorCode, 0);

                // 是否還在冷卻時間?
                string phoneStr = "0" + phone;
                string key = $"local:phoneOtp:phone:{phoneStr}";
                string referenceId = redisService.GetValue<string>(key);

                // 找的到 redis 的資料代表此功能還在冷卻時間
                if (referenceId != null)
                {
                    TimeSpan? ttl = redisService.GetRemainingTime(referenceId);

                    if (ttl.HasValue) return (ErrorCodeDefine.OtpCooldown, (int)ttl.Value.TotalSeconds);

                    return (ErrorCodeDefine.OtpCooldown, 0);
                }

                // 發送簡訊 OTP
                OtpSendResponse response = otpService.SendOtp(phoneStr, otpExpiry.ToString());

                if (response != null && response.Status == true)
                {
                    redisService.SetValue(key, response.ReferenceId, TimeSpan.FromSeconds(otpExpiry));
                    return (ErrorCodeDefine.Success, otpExpiry);
                }

                return (ErrorCodeDefine.ServerError, 0);
            }
            catch (Exception)
            {
                throw;
            }
        }

        // TODO: 單元測試
        public (ErrorCodeDefine, int ttl) VerifyPhone(RequestVerifyOtpDto verifyPhoneDto)
        {
            try
            {
                MemberSession memberSession = sessionService.GetSession<MemberSession>(memberSessionKey);
                (int errorCodeNumber, int phone) = memberRepository.GetPhoneAtVerifyPhone(memberSession.MemberId);

                // 沒有拿到使用者電話
                if (phone == -1) return (ErrorCodeDefine.ServerError, 0);
                // 如果沒有被定義在 enum 裡
                if (!Enum.IsDefined(typeof(ErrorCodeDefine), errorCodeNumber)) return (ErrorCodeDefine.ServerError, 0);

                ErrorCodeDefine errorCode = (ErrorCodeDefine)errorCodeNumber;
                // 號碼已驗證 或其他狀況
                if (errorCode != ErrorCodeDefine.Success) return (errorCode, 0);

                string phoneStr = "0" + phone;
                string key = $"local:phoneOtp:phone:{phoneStr}";
                string referenceId = redisService.GetValue<string>(key);
                // 是否過期?
                if (referenceId == null) return (ErrorCodeDefine.VerifyFail, 0);

                // 驗證是否成功
                bool successFlag = otpService.VerifyOtp(referenceId, verifyPhoneDto.OtpCode);

                // 剩餘時間
                TimeSpan? ttl = redisService.GetRemainingTime(referenceId);
                int remainingSeconde = 0;
                if (ttl.HasValue) remainingSeconde = (int)ttl.Value.TotalSeconds;

                if (!successFlag) return (ErrorCodeDefine.VerifyFail, remainingSeconde);

                bool repositorySuccess = memberRepository.EditPhoneVerified(memberSession.MemberId);

                if (!repositorySuccess) return (ErrorCodeDefine.VerifyFail, remainingSeconde);

                return (ErrorCodeDefine.Success, remainingSeconde);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}