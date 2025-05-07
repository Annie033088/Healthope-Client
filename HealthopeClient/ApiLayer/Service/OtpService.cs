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
using Pipelines.Sockets.Unofficial.Arenas;
using ApiLayer.Interface;

namespace ApiLayer.Service
{
    // 參考文件: https://developer.telesign.com/enterprise/docs/verify-api-verify-with-telesign-code
    public class OtpService:IOtpService
    {
        //private readonly string customerId = "CUSTOMER_ID"; // 申請時的帳號 Id
        //private readonly string apiKey = "API_KEY"; // 申請時拿到的 Key
        private readonly HttpClient httpClient;

        public OtpService()
        {
            httpClient = new HttpClient();
            //string authString = $"{customerId}:{apiKey}";
            //byte[] authBytes = Encoding.ASCII.GetBytes(authString);
            //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authBytes));
        }

        // TODO: 單元測試
        public OtpSendResponse SendOtp(string phoneNumber, string expiry)
        {
            try
            {
                string url = "https://localhost:44395/Otp/SendOtp";
                Dictionary<string, string> dictionaryContent = new Dictionary<string, string>
                {
                    { "PhoneNumber", phoneNumber },
                    { "TTL", expiry }
                };
                string json = JsonConvert.SerializeObject(dictionaryContent);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = httpClient.PostAsync(url, content).Result;
                string responseString = response.Content.ReadAsStringAsync().Result;

                OtpSendResponse otpResponse = JsonConvert.DeserializeObject<OtpSendResponse>(responseString);
                return otpResponse;
            }
            catch (Exception)
            {
                throw;
            }
        }

        // TODO: 單元測試
        public bool VerifyOtp(string referenceId, string otpCode)
        {
            try
            {
                string url = "https://localhost:44395/Otp/VerifyOtp";
                Dictionary<string, string> dictionaryContent = new Dictionary<string, string>
                {
                    { "ReferenceId", referenceId },
                    { "OtpCode", otpCode }
                };
                string json = JsonConvert.SerializeObject(dictionaryContent);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = httpClient.PostAsync(url, content).Result;
                string responseString = response.Content.ReadAsStringAsync().Result;
                OtpVerifyResponse responseObj = JsonConvert.DeserializeObject<OtpVerifyResponse>(responseString);

                return responseObj.Status;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}