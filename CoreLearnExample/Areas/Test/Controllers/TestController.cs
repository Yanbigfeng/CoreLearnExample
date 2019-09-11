using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CoreLearnExample.Util;
using Microsoft.AspNetCore.Mvc;

namespace CoreLearnExample.Areas.Test.Controllers
{
    public class TestController : Controller
    {
        public HttpRequestUtil _httpRequestUtil;
        private readonly IHttpClientFactory _clientFactory;

        public TestController(HttpRequestUtil httpRequestUtil, IHttpClientFactory clientFactory)
        {
            _httpRequestUtil = httpRequestUtil;
            _clientFactory = clientFactory;
        }
        [Area("Test")]
        public async Task<IActionResult> Index()
        {

            try
            {

                var request = new HttpRequestMessage(HttpMethod.Get,
                "https://api.github.com/repos/aspnet/AspNetCore.Docs/branches");
                request.Headers.Add("Accept", "application/vnd.github.v3+json");
                request.Headers.Add("User-Agent", "HttpClientFactory-Sample");

                var client = _clientFactory.CreateClient();

                var response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var strResult = await response.Content
                          .ReadAsStringAsync();
                }
                else
                {

                }



                string url = "www.baidu.com";
                var result = await _httpRequestUtil.Get("https://api.github.com/repos/aspnet/AspNetCore.Docs/branches");


                var posrResulit =await  _httpRequestUtil.Post("http://www.baidu.com/api/getuserinfo", "userid=23456798");

            }
            catch (Exception ex) {
            }



            return View();
        }
    }
}