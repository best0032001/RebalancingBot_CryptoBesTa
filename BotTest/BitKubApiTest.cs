using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using RebalancingBot.Model;
using RebalancingBot.Model.BitkubModel;

namespace BotTest
{
    [TestClass]
    public class BitKubApiTest
    {
        String API_HOST = "https://api.bitkub.com";
        String API_KEY = "-";
        String API_SECRET = "-";

        [TestMethod]
        public async Task TestBalances()
        {
            HttpClient _client = new HttpClient();
            String API = API_HOST + "/api/servertime";
            var response = await _client.GetAsync(API);
            response.EnsureSuccessStatusCode();
            String responseString = await response.Content.ReadAsStringAsync();
            Int32 ts = Convert.ToInt32(responseString);
           
            TSModel tSModel = new TSModel();
            tSModel.ts = ts;
            String json = JsonConvert.SerializeObject(tSModel);


            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] keyBytes = encoding.GetBytes(API_SECRET);
            byte[] messageBytes = encoding.GetBytes(json);
            System.Security.Cryptography.HMACSHA256 cryptographer = new System.Security.Cryptography.HMACSHA256(keyBytes);
            byte[] bytes = cryptographer.ComputeHash(messageBytes);
            String sig = BitConverter.ToString(bytes).Replace("-", "").ToLower();
            Payload payload = new Payload();
            payload.ts = ts;
            payload.sig = sig;
            json = JsonConvert.SerializeObject(payload);
            _client.DefaultRequestHeaders.Add("X-BTK-APIKEY", API_KEY);
            var content = new StringContent(json);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
           
            API = API_HOST + "/api/market/balances";
            response = await _client.PostAsync(API, content);
            responseString = await response.Content.ReadAsStringAsync();
            Assert.IsTrue((int)response.StatusCode == 200);

        }
        [TestMethod]
        public async Task TestMarketBids()
        {
            HttpClient _client = new HttpClient();
            String API = API_HOST + "/api/market/bids?sym=THB_KUB&lmt=1";
            var response = await _client.GetAsync(API);
            response.EnsureSuccessStatusCode();
            String responseString = await response.Content.ReadAsStringAsync();
            ResponseMarketBidModel responseMarketBidModel = JsonConvert.DeserializeObject<ResponseMarketBidModel>(responseString);
            Assert.IsTrue((int)response.StatusCode == 200);

        }

    }
}
