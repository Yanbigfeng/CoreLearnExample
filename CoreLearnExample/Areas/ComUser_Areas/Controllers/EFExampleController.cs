using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreLearnExample.EData;
using Microsoft.AspNetCore.Mvc;

namespace CoreLearnExample.Areas.ComUser_Areas.Controllers
{
    [Area("ComUser_Areas")]
    public class EFExampleController : Controller
    {
        //构造函数注入
        public testContext _context;
        public EFExampleController(testContext context) //依赖注入得到实例  
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var viewMode = _context.Set<TableTest>().ToList();

            return View(viewMode);
        }

        //增加
        public void Add(string Introduce, string Describe)
        {

            TableTest tableTestAdd = new TableTest();
            tableTestAdd.Id = Guid.NewGuid().ToString("N");
            tableTestAdd.Introduce = Introduce;
            tableTestAdd.Describe = Describe;
            tableTestAdd.AddTime = DateTime.Now;

            _context.Set<TableTest>().Add(tableTestAdd);
            _context.SaveChanges();

        }
        //修改
        public void Edit(string id, string Introduce, string Describe)
        {

            TableTest tableTestAdd = _context.Set<TableTest>().Where(u => u.Id == id).FirstOrDefault();
            tableTestAdd.Introduce = Introduce;
            tableTestAdd.Describe = Describe;
            _context.Set<TableTest>().Update(tableTestAdd);
            _context.SaveChanges();

        }
        //删除
        public void Del(string id)
        {
            TableTest tableTestAdd = _context.Set<TableTest>().Where(u => u.Id == id).FirstOrDefault();
            _context.Set<TableTest>().Remove(tableTestAdd);
            _context.SaveChanges();

        }
    }
}