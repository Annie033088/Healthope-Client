using PersistentLayer.Models;

namespace PersistentLayer.Interface
{
    public interface IMemberRepository
    {
        /// <summary>
        /// 註冊會員
        /// </summary>
        (int errorCodeNum, int memberId) AddMember(AddMemberDto addMemberDto);

        /// <summary>
        /// 取得會員手機
        /// </summary>
        (int errorCodeNum, int phone) GetPhoneAtVerifyPhone(int memberId);

        /// <summary>
        /// 修改會員手機認證狀態 ( 若第三方 OTP 簡訊服務商回傳成功的話，改為驗證通過 )
        /// </summary>
        bool EditPhoneVerified(int memberId);
    }
}
