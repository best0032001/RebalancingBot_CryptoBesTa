using Newtonsoft.Json;
using RebalancingBot.Model.BitkubModel;
using RebalancingBot.Model.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RebalancingBot.Model.Repository
{
    public class BitkubRepository : IBitkubRepository
    {
        private readonly IHttpClientFactory _clientFactory;
        private String API_HOST = "https://api.bitkub.com";
        private String API_KEY = "-";
        private String API_SECRET = "-";
        private ILineNotiRepository _lineNotiRepository;
        public BitkubRepository(IHttpClientFactory clientFactory, ILineNotiRepository lineNotiRepository)
        {
            _clientFactory = clientFactory;
            API_KEY = Environment.GetEnvironmentVariable("API_KEY");
            API_SECRET = Environment.GetEnvironmentVariable("API_SECRET");
            _lineNotiRepository = lineNotiRepository;
        }
        public async Task Worker()
        {
            Console.WriteLine("Start");
            ResponseBalancesModel responseBalancesModel = await getBalances();
            await _lineNotiRepository.sendLineNoti(getMessageBalances(responseBalancesModel));
        }

        public async Task<ResponseBalancesModel> getBalances()
        {
            ResponseBalancesModel responseBalancesModel = null;

            HttpClient _client = _clientFactory.CreateClient();
            TSModel tSModel = await getServerTime();
            Payload payload = new Payload();
            payload.ts = tSModel.ts;
            payload.sig = await getSig(tSModel);
            String json = JsonConvert.SerializeObject(payload);
            _client.DefaultRequestHeaders.Add("X-BTK-APIKEY", API_KEY);
            var content = new StringContent(json);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            String API = API_HOST + "/api/market/balances";
            var response = await _client.PostAsync(API, content);
            var responseString = await response.Content.ReadAsStringAsync();
            responseBalancesModel = JsonConvert.DeserializeObject<ResponseBalancesModel>(responseString);
            return responseBalancesModel;
        }

        public async Task<TSModel> getServerTime()
        {
            HttpClient _client = _clientFactory.CreateClient();
            String API = API_HOST + "/api/servertime";
            var response = await _client.GetAsync(API);
            response.EnsureSuccessStatusCode();
            String responseString = await response.Content.ReadAsStringAsync();
            Int32 ts = Convert.ToInt32(responseString);
            TSModel tSModel = new TSModel();
            tSModel.ts = ts;
            return tSModel;
        }

        public async Task<String> getSig(TSModel tSModel)
        {
            String json = JsonConvert.SerializeObject(tSModel);
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] keyBytes = encoding.GetBytes(API_SECRET);
            byte[] messageBytes = encoding.GetBytes(json);
            System.Security.Cryptography.HMACSHA256 cryptographer = new System.Security.Cryptography.HMACSHA256(keyBytes);
            byte[] bytes = cryptographer.ComputeHash(messageBytes);
            String sig = BitConverter.ToString(bytes).Replace("-", "").ToLower();
            return sig;
        }

        public String getMessageBalances(ResponseBalancesModel responseBalancesModel)
        {
            Double THB = responseBalancesModel.result.THB.available + responseBalancesModel.result.THB.reserved;
            Double KUB = responseBalancesModel.result.KUB.available + responseBalancesModel.result.KUB.reserved;
            String message = "THB:"+ THB +" KUB:"+ KUB;
            return message;
        }
    }
}
