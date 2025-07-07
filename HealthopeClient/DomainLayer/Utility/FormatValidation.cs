using System;
using System.Text.RegularExpressions;

namespace DomainLayer.Utility
{
    public class FormatValidation
    {
        /// <summary>
        /// 驗證帳號
        /// </summary>
        public bool ValidAccount(string account)
        {
            // 8~20 位英文數字
            string AccountRegex = "^(?=.*[a-zA-Z])(?=.*\\d)[a-zA-Z\\d]{8,20}$";
            return Regex.IsMatch(account, AccountRegex);
        }

        /// <summary>
        /// 驗證密碼
        /// </summary>
        public bool ValidPwd(string pwd)
        {
            // 8~20 位英文數字
            string PwdRegex = "^(?=.*[a-zA-Z])(?=.*\\d)[a-zA-Z\\d]{8,20}$";
            return Regex.IsMatch(pwd, PwdRegex);
        }

        /// <summary>
        /// 驗證信箱
        /// </summary>
        public bool ValidEmail(string email)
        {
            // 可空
            if (String.IsNullOrEmpty(email)) return true;

            // [^\s@] 代表至少一個不是空白或 @ 的字元
            string emailRegex = "^[^\\s@]+@[^\\s@]+\\.[^\\s@]+$"; // EX: abc@ewq.ee
            if (email.Length > 254) return false; // 規定總長最長 254

            string[] parts = email.Split('@');
            if (parts.Length != 2) return false;

            string localPart = parts[0];
            string domain = parts[1];

            if (
              localPart.Length < 3 || // 建議最少 3 字元
              localPart.Length > 64 || // 規定 @以前 最長 64
              domain.Length > 251 // 不得超過 254 - 3
            )
            {
                return false;
            }

            return Regex.IsMatch(email, emailRegex);
        }

        /// <summary>
        /// 驗證有效手機號碼
        /// </summary>
        public bool ValidPhone(int phone)
        {
            string phoneRegex = "^9\\d{8}$"; // 9 開頭, 加後 8 位數
            return Regex.IsMatch(phone.ToString(), phoneRegex);
        }
    }
}
