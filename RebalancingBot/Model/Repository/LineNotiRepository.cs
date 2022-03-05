using RebalancingBot.Model.Interface;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace RebalancingBot.Model.Repository
{
    public class LineNotiRepository : ILineNotiRepository
    {


        private readonly IHttpClientFactory _clientFactory;
        private String _lineToken;
        public LineNotiRepository(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
            _lineToken = Environment.GetEnvironmentVariable("LINE_TOKEN");
        }
        public async Task sendLineNoti(string message)
        {

            String lineURL = "https://notify-api.line.me/api/notify";
            var postData = new Dictionary<string, string>
            {
                { "message", message }
            };
            HttpClient httpClient = _clientFactory.CreateClient();
            var content = new FormUrlEncodedContent(postData);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/x-www-form-urlencoded");
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _lineToken);
            HttpResponseMessage response = await httpClient.PostAsync(lineURL, content);
            response.EnsureSuccessStatusCode();
        }
    }
}
