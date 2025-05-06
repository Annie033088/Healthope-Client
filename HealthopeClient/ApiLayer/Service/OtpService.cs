using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Collections;
using Newtonsoft.Json;
using ApiLayer.Models.ThirdParty;

namespace ApiLayer.Service
{
    // 參考文件: https://developer.telesign.com/enterprise/docs/verify-api-verify-with-telesign-code
    public class OtpService
    {
        private readonly string customerId = "CUSTOMER_ID"; // 申請時的帳號 Id
        private readonly string apiKey = "API_KEY"; // 申請時拿到的 Key
        private readonly HttpClient httpClient;

        public OtpService()
        {
            httpClient = new HttpClient();
            string authString = $"{customerId}:{apiKey}";
            byte[] authBytes = Encoding.ASCII.GetBytes(authString);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authBytes));
        }

        public string SendOtp(string phoneNumber)
        {
            string url = "https://rest-api.telesign.com/v1/verify/sms";
            FormUrlEncodedContent content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("phone_number", phoneNumber),
                new KeyValuePair<string, string>("language", "zh-Hant"),
                new KeyValuePair<string, string>("ttl", "180") // 3 分鐘有效
            });

            HttpResponseMessage response = httpClient.PostAsync(url, content).Result;
            string responseString = response.Content.ReadAsStringAsync().Result;

            return responseString;
        }

        public bool verifyOtp(string referenceId, string otpCode)
        {
            Dictionary<string, string> payload = new Dictionary<string, string>
            {
                { "reference_id", referenceId },
                { "verify_code", otpCode }
            };
            string json = JsonConvert.SerializeObject(payload);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = httpClient.PostAsync("https://rest-api.telesign.com/v1/verify/code", content).Result;
            string result = response.Content.ReadAsStringAsync().Result;
            OtpVerifyResponse responseObj = JsonConvert.DeserializeObject<OtpVerifyResponse>(result);

            if (responseObj.status.code == 300) return true;

            return false;
        }
    }
}