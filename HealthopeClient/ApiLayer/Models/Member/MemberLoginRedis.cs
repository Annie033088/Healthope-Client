using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiLayer.Models.Member
{
    public class MemberLoginRedis
    {
        /// <summary>
        /// redis 存的 sessionId
        /// </summary>
        public string SessionId { get; set; }

        /// <summary>
        /// 檢查 權限/狀態 是否被異動
        /// </summary>
        public ErrorCodeDefine ErrorCode { get; set; }
    }
}