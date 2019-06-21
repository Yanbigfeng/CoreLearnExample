using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreLearnExample.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace CoreLearnExample.Controllers
{
    //配置文件的读取
    public class AppSetReadController : Controller
    {

        //构造函数注入
        //启动是为了访问配置文件定义的变量
        public IConfiguration _Configuration { get; }
        public AppSetReadController(IConfiguration Configuration) //依赖注入得到实例  
        {
            _Configuration = Configuration;
        }
        public IActionResult Index()
        {
            //在启动时候访问配置文件
            var appValue = _Configuration.GetValue<string>("AppSetValue", "");
            //绑定到类
            PeserModel peserModel = new PeserModel();
            _Configuration.GetSection("PeserModel").Bind(peserModel);

            //多层类嵌套的绑定
            AppSetModel appSetModel = new AppSetModel();
            _Configuration.GetSection("AppSetModel").Bind(appSetModel);
            AppSetModel setModel = _Configuration.GetSection("AppSetModel").Get<AppSetModel>();
            //读取数组
            AppSetArrayModel setArrayModel = new AppSetArrayModel();
            _Configuration.GetSection("AppSetArrayModel").Bind(setArrayModel);
            ViewBag.appValue = appValue;
            ViewBag.peserModel = peserModel;
            ViewBag.appSetModel = appSetModel;
            ViewBag.setArrayModel = setArrayModel;
            return View();
        }
    }
}