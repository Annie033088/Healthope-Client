using ApiLayer.Models;
using ApiLayer.Models.AccountAccess.RequestMemberDto;

namespace ApiLayer.Interface
{
    public interface IAccountAccessService
    {
        /// <summary>
        /// 註冊會員
        /// </summary>
        ErrorCodeDefine AddMember(RequestAddMemberDto requestAddMemberDto);

        /// <summary>
        /// 會員登出
        /// </summary>
        bool MemberLogout();
    }
}
