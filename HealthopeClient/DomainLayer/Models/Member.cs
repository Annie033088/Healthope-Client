using System;

namespace DomainLayer.Models
{
    public class Member
    {
        /// <summary>
        /// 會員 Id
        /// </summary>
        public int MemberId { get; set; }

        /// <summary>
        /// 帳號
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 密碼加密後的雜湊 (salt(36) + 加密(64))
        /// </summary>
        public string Hash { get; set; }

        /// <summary>
        /// 信箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 手機
        /// </summary>
        public int Phone { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 照片路徑
        /// </summary>
        public string PhotoUrl { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        public DateTime Birthday { get; set; }

        /// <summary>
        /// 性別
        /// </summary>
        public short Gender { get; set; }

        /// <summary>
        /// 身高
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// 體重
        /// </summary>
        public int Weight { get; set; }

        /// <summary>
        /// 狀態
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// 今年的缺席次數
        /// </summary>
        public short AbsenceTime { get; set; }

        /// <summary>
        /// 允許參加團體課程的開始日期 (因爽約而被禁止)
        /// </summary>
        public DateTime AllowGroupClass { get; set; }

        /// <summary>
        /// 會籍到期日
        /// </summary>
        public DateTime MembershipExpiry { get; set; }

        /// <summary>
        /// 創建時間
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 更新時間
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }
}
