using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CoreLearnExample.Models;

namespace CoreLearnExample.Controllers
{
    /// <summary>
    /// session扩展示例
    /// </summary>
    public class SessionController : Controller
    {
        public IActionResult Index()
        {
            //dll序列化
            // UserViewModel viewModel=   HttpContext.Session.Get<UserViewModel>("userLogin");
            //流文件序列化
            UserViewModel viewModel = HttpContext.Session.GetStream<UserViewModel>("userLogin");
            if (viewModel == null) {
                viewModel = new UserViewModel();
            }
            return View(viewModel);
        }

        public void Set(String name ,int age)
        {
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

    }
}