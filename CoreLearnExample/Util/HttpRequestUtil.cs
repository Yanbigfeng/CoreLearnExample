using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CoreLearnExample.Util
{
    /// <summary>
    /// Http请求帮助类
    /// </summary>

    //类型化客户端模式
    public class HttpRequestUtil
    {
        //构造函数注入
        private readonly HttpClient _client;
        public HttpRequestUtil(HttpClient client)
        {

            // client.BaseAddress = new Uri("https://api.github.com/");
            //表头参数
            client.DefaultRequestHeaders.Add("Accept",
           "application/vnd.github.v3+json");
            client.DefaultRequestHeaders.Add("User-Agent",
                "HttpClientFactory-Sample");
            _client = client;
        }

        public async Task<string> Get(string url)
        {
            //string url = "https://api.github.com//repos/aspnet/AspNetCore.Docs/issues?state=open&sort=created&direction=desc";
            var response = await _client.GetAsync(url);

            var status = response.EnsureSuccessStatusCode();

            var result = await response.Content
                .ReadAsStringAsync();



            return result;

        }

        public async Task<string> Post(string url, string dataJson)
        {

            HttpContent httpContent = new JsonContent(dataJson);
            var response = await _client.PostAsync(url, httpContent);

            var status = response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();

            return result;

        }

        public class JsonContent : StringContent
        {
            public JsonContent(object obj) :
               base(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json")
            { }
        }

    }


}
