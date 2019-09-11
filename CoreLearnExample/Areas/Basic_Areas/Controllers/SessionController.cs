using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CoreLearnExample.ViewModels;
using CoreLearnExample.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using CoreLearnExample.Filter;

namespace CoreLearnExample.Areas.Basic_Areas.Controllers
{
    [Area("Basic_Areas")]
    public class SessionController : Controller
    {

        private readonly IDistributedCache _cache;

        public SessionController(IDistributedCache cache)
        {
            _cache = cache;
        }

        public IActionResult Index()
        {
            //dll序列化
            // UserViewModel viewModel=   HttpContext.Session.Get<UserViewModel>("userLogin");
            //流文件序列化
            UserViewModel viewModel = HttpContext.Session.GetStream<UserViewModel>("userLogin");
            if (viewModel == null)
            {
                viewModel = new UserViewModel();
            }
            var b = _cache.GetString("b");
            var c = HttpContext.Session.GetString("c");
            ViewBag.nowTime = DateTime.Now;
            return View(viewModel);
        }
        public void Set(String name, int age)
        {
            _cache.SetString("b", "bbbb2");

            HttpContext.Session.SetString("c", "cccc");
            UserViewModel viewModel = new UserViewModel()
            {
                name = name,
                age = age
            };
            //dll序列化
            // HttpContext.Session.Set<UserViewModel>("userLogin", viewModel);
            //流文件序列化
            HttpContext.Session.SetStream<UserViewModel>("userLogin", viewModel);
            // return View(viewModel);
        }

        public void RedisSet(String name, int age)
        {
            _cache.SetString("b", name);

            HttpContext.Session.SetString("c", name);

            // return View(viewModel);
        }

        /******************************************缓存********************************************/
        #region 缓存页面
        //[ResponseCache(Duration = 5)]
         //[ResponseCache(VaryByHeader = "User-Agent", Duration = 30)]
        //[ResponseCache(CacheProfileName = "Default30")]
        [MyResourceFilter]
        public IActionResult CacheView()
        {
            ViewBag.time = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            return View();
        }
        #endregion

        #region 缓存页面
        //[ResponseCache(Duration = 5)]
        //[ResponseCache(VaryByHeader = "User-Agent", Duration = 30)]
        //[ResponseCache(CacheProfileName = "Default30")]
        [MyResourceFilter]
        public IActionResult CacheView2()
        {
            ViewBag.time = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            return View();
        }
        #endregion
    }
}