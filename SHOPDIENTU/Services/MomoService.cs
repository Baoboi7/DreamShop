using Newtonsoft.Json.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SHOPDIENTU.Services
{
    public class MomoService
    {
        private readonly string endpoint = "https://test-payment.momo.vn/v2/gateway/api/create";
        private readonly string partnerCode = "MOMO";
        private readonly string accessKey = "F8BBA842ECF85";
        private readonly string secretKey = "K951B6PE1waDMi640xX08PD3vg6EkVlz";

        public string CreatePayment(string orderId, long amount)
        {
            string requestId = Guid.NewGuid().ToString();
            string orderInfo = "Thanh toán đơn hàng DreamShop";
            string redirectUrl = "https://localhost:44323/Checkout/MomoReturn";
            string ipnUrl = "https://localhost:44323/Checkout/MomoReturn";

            string rawHash =
                "accessKey=" + accessKey +
                "&amount=" + amount +
                "&extraData=" +
                "&ipnUrl=" + ipnUrl +
                "&orderId=" + orderId +
                "&orderInfo=" + orderInfo +
                "&partnerCode=" + partnerCode +
                "&redirectUrl=" + redirectUrl +
                "&requestId=" + requestId +
                "&requestType=captureWallet";

            string signature = SignSHA256(rawHash, secretKey);

            JObject message = new JObject
            {
                { "partnerCode", partnerCode },
                { "partnerName", "DreamShop" },
                { "storeId", "DreamShopStore" },
                { "requestId", requestId },
                { "amount", amount },
                { "orderId", orderId },
                { "orderInfo", orderInfo },
                { "redirectUrl", redirectUrl },
                { "ipnUrl", ipnUrl },
                { "lang", "vi" },
                { "extraData", "" },
                { "requestType", "captureWallet" },
                { "signature", signature }
            };

            using (var client = new HttpClient())
            {
                var content = new StringContent(message.ToString(), Encoding.UTF8, "application/json");
                var response = client.PostAsync(endpoint, content).Result;
                string result = response.Content.ReadAsStringAsync().Result;

                JObject json = JObject.Parse(result);
                return json["payUrl"].ToString();
            }
        }

        private static string SignSHA256(string message, string key)
        {
            var encoding = new UTF8Encoding();
            byte[] keyByte = encoding.GetBytes(key);
            byte[] messageBytes = encoding.GetBytes(message);

            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
                return BitConverter.ToString(hashmessage).Replace("-", "").ToLower();
            }
        }
    }
}
