namespace PersistentLayer.Models
{
    public class AddMemberDto
    {
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
    }
}
