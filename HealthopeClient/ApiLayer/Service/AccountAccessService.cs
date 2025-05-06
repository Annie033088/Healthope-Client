using System;
using System.Security.Principal;
using ApiLayer.Interface;
using ApiLayer.Models;
using ApiLayer.Models.AccountAccess.RequestMemberDto;
using ApiLayer.Models.Member;
using DomainLayer.Utility;
using PersistentLayer.Interface;
using PersistentLayer.Models;

namespace ApiLayer.Service
{
    public class AccountAccessService : IAccountAccessService
    {
        private readonly string memberSessionKey = "memberSessionKey";
        private readonly IMemberRepository memberRepository;
        private readonly IRedisService redisService;
        private readonly ISessionService sessionService;

        public AccountAccessService(IMemberRepository memberRepository,IRedisService redisService,ISessionService sessionService)
        {
            this.memberRepository = memberRepository;
            this.redisService = redisService;
            this.sessionService = sessionService;
        }

        /// <summary>
        /// 註冊會員
        /// </summary>
        public ErrorCodeDefine AddMember(RequestAddMemberDto requestAddMemberDto)
        {
            try
            {
                // cookie/redis 過期時間
                TimeSpan expiry = TimeSpan.FromHours(12);

                // 加密工具
                Hash hashUtility = new Hash();
                string salt = hashUtility.GenerateSalt();
                AddMemberDto addMemberDto = new AddMemberDto()
                {
                    Account = requestAddMemberDto.Account,
                    Hash = hashUtility.PwdHash(requestAddMemberDto.Pwd, salt),
                    Email = requestAddMemberDto.Email,
                    Phone = int.Parse(requestAddMemberDto.Phone)
                };

                (int errorCodeNumber, int memberId) = memberRepository.AddMember(addMemberDto);

                // 如果沒有被定義在 enum 裡
                if (!Enum.IsDefined(typeof(ErrorCodeDefine), errorCodeNumber))
                    return ErrorCodeDefine.ServerError;

                ErrorCodeDefine errorCode = (ErrorCodeDefine)errorCodeNumber;

                // 註冊成功順便登入
                if (errorCode == ErrorCodeDefine.Success)
                {
                    // asp.net 儲存會話資料
                    sessionService.SaveSession(memberSessionKey, new MemberSession() { MemberId = memberId });

                    // redis 儲存登入後的 sessionId 用來判斷後踢前，儲存 ErrorCode 來判斷權限/狀態是否被異動
                    // 過期時間跟 state server 一致為 12 小時
                    string memberLoginRdisKey = "MemberLogin" + memberId;
                    MemberLoginRedis memberLoginRedis = new MemberLoginRedis() { SessionId = sessionService.GetSessionId(), ErrorCode = ErrorCodeDefine.Success };
                    redisService.SetValue(memberLoginRdisKey, memberLoginRedis, expiry);
                    return errorCode;
                }

                return errorCode;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 會員登出
        /// </summary>
        public bool MemberLogout()
        {
            try
            {
                MemberSession memberSession = sessionService.GetSession<MemberSession>(memberSessionKey);

                string redisKey = "MemberLogin" + memberSession.MemberId;

                // 清除會話
                sessionService.ClearSerssion();

                // 清除 redis 的登入後資料
                redisService.DeleteKey(redisKey);

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}