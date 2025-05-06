using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiLayer.Models.ThirdParty
{
    public class OtpVerifyResponse
    {
        public string reference_id { get; set; }
        public OtpVerifyStatus status { get; set; }
    }

    public class OtpVerifyStatus
    {
        public int code { get; set; }
        public string description { get; set; }
    }
}