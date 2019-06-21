using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CoreLearnExample.Lognet;

namespace CoreLearnExample.Controllers
{
    /// <summary>
    /// lognet4的集成使用
    /// </summary>
    public class LogNetController : Controller
    {
        public IActionResult Index()
        {
            throw new Exception("自定义异常！！！");  //有异常则会记录到logfile文件夹中

            //LogHelper.Info("这是记录信息");

            return View();
        }
    }
}