using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiLayer.Models.Member.ReponseMemberDto
{
    public class ResponseOtpRemainingTime
    {
        /// <summary>
        /// 剩餘(冷卻)時間
        /// </summary>
        public int RemainingSecond { get; set; }
    }
}