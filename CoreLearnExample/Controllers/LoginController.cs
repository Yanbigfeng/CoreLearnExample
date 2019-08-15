using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CoreLearnExample.Filter;
using Microsoft.AspNetCore.Http;
using CoreLearnExample.ViewModels;

namespace CoreLearnExample.Controllers
{
    /// <summary>
    /// 登录验证控制器（授权的使用）
    /// </summary>
    [Skip]
    public class LoginController : Controller
    {


        /// <summary>
        /// 登录方法
        /// </summary>
        /// <returns></returns>
        /// 
        [MyAuthorizeFilter]
        public IActionResult Login()
        {
            string userName = HttpContext.Session.GetString("userName");
            if (string.IsNullOrEmpty(userName))
            {
                userName = "没有值";
            }
            ViewBag.userName = userName;
            return View();
        }

    
        public IActionResult LogOut()
        {

            return View();
        }

        /******************************************缓存********************************************/


        public void SetSession(String name)
        {
            HttpContext.Session.SetString("userName", name);
        }

        public void ClearSession()
        {
            HttpContext.Session.Remove("userName");
        }
    }
}