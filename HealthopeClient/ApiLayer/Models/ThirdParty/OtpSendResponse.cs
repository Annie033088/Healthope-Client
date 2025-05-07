using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiLayer.Models.ThirdParty
{
    public class OtpSendResponse
    {
        public string ReferenceId { get; set; }
        public bool Status { get; set; }
        public string Phone { get; set; }

    }
}