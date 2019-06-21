using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CoreLearnExample.Models;

namespace CoreLearnExample.Controllers
{
    /// <summary>
    /// 框架依赖注入
    /// </summary>
    public class DICycleController : Controller
    {

        /**************************************************** 默认框架注入***************************************/
        //构造函数注入
        public CycleService CycleService;
        public ICycleTransient TransientCycle { get; }
        public ICycleScoped ScopedCycle { get; }
        public ICycleSingleton SingletonCycle { get; }
        public ICycleSingletonInstance SingletonInstanceCycle { get; }
        public DICycleController(
                               CycleService cycleService,
                               ICycleTransient transientCycle,
                               ICycleScoped scopedCycle,
                               ICycleSingleton singletonCycle,
                               ICycleSingletonInstance singletonInstanceCycle)
        {
            CycleService = cycleService;
            TransientCycle = transientCycle;
            ScopedCycle = scopedCycle;
            SingletonCycle = singletonCycle;
            SingletonInstanceCycle = singletonInstanceCycle;
        }

        public IActionResult Index()
        {
            ViewBag.CycleService = CycleService;
            ViewBag.TransientCycle = TransientCycle.CycleId;

            ViewBag.ScopedCycle = ScopedCycle.CycleId;
            ViewBag.SingletonCycle = SingletonCycle.CycleId;
            ViewBag.SingletonInstanceCycle = SingletonInstanceCycle.CycleId;

            return View();
        }


        /*******************************************AutoFac框架替换默认注入*********************************************/


        //使用第三方容器的高级输入属性注入
        public ICycleTransient _testService { get; set; }


        public ActionResult AutoFacView()
        {

            ViewBag.CycleService = CycleService;
            ViewBag.TransientCycle = TransientCycle.CycleId;

            ViewBag.ScopedCycle = ScopedCycle.CycleId;
            ViewBag.SingletonCycle = SingletonCycle.CycleId;
            ViewBag.SingletonInstanceCycle = SingletonInstanceCycle.CycleId;

            //属性注入测试
            ViewBag.attribute=  _testService.CycleId;

            return View();
        }
    }
}