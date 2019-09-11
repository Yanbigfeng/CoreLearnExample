using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CoreLearnExample.EData;

namespace CoreLearnExample.Areas.ComUser_Areas.Controllers
{

    /// <summary>
    /// 证明EF生命周期控制器
    /// </summary>
    [Area("ComUser_Areas")]
    public class EFLifeCycleController : Controller
    {

        //构造函数注入
        public testContext _context;
        public testContext _context2;


        //单例模式分页传值
        public testContext _context3;
        public EFLifeCycleController(testContext context, testContext context2) //依赖注入得到实例  
        {
            _context = context;
            _context2 = context2;
        }


        #region EF生命周期首页
        public IActionResult EFLifeCycleIndex([FromServices] testContext context)
        {
            #region 原测试代码
            //验证瞬时周期，直接创建两个看是否一样即可
            //bool isSameDb = false;
            //if (_context == _context2)
            //{
            //    isSameDb = true;
            //}
            //else
            //{
            //    isSameDb = false;
            //}
            ////增加测试----瞬时的另一个上下文不能提交保存其他的数据
            ////请求内单例和全局都可以保存
            ////然后区分请求单例和全局单例 在另一个方法提交保存请求单例不能保存全局单例可以保存
            //TableTest tableTestAdd = new TableTest();
            //tableTestAdd.Id = Guid.NewGuid().ToString("N");
            //tableTestAdd.Introduce = "周期测试";
            //tableTestAdd.Describe = "周期测试";
            //tableTestAdd.AddTime = DateTime.Now;
            //_context.Set<TableTest>().Add(tableTestAdd);
            ////_context2.SaveChanges(); 
            //using (var db = new testContext())
            //{
            //    var i = 1;
            //    db.Set<TableTest>().ToList();
            //    var c = 2;
            //}
            #endregion


            #region 单例证明1
            var singleton1 = Singleton.GetInstance(_context);
            singleton1.SetNull();
            var singleton2 = Singleton.GetInstance(_context2);
            singleton2.SetNull();
            bool isSameDb = false;
            if (singleton1._db == singleton2._db)
            {
                isSameDb = true;
            }
            else
            {
                isSameDb = false;
            }
            _context3 = singleton1._db;
            #endregion

            #region 单例证明2

            //var singleton1 = Singleton.GetInstance(_context);
            //bool isSameDb = false;
            //if (singleton1._db == _context)
            //{
            //    isSameDb = true;
            //}
            //else
            //{
            //    isSameDb = false;
            //}
            #endregion
            return View(isSameDb);
        }
        #endregion

        #region 生命周期请求内单例
        public IActionResult AddScopedView()
        {
            #region 原测试代码
            //全局单例可以保存
            //_context2.SaveChanges(); 
            #endregion

            #region 单例证明1

            bool isSameDb = false;
            var singleton3 = Singleton.GetInstance(_context2);
            singleton3.SetNull();
            if (_context3 == singleton3._db)
            {
                isSameDb = true;
            }
            else
            {
                isSameDb = false;
            }

            #endregion

            #region 单例证明2
            //bool isSameDb= false;
            //var singleton1 = Singleton.GetInstance(_context);
            //if (singleton1._db == _context)
            //{
            //    isSameDb = true;
            //}
            //else
            //{
            //    isSameDb = false;
            //}
            #endregion


            return View(isSameDb);
        }
        #endregion
    }
    #region 单例模式1
    public class Singleton
    {
        private static Singleton instance;
        public testContext _db;

        private Singleton([FromServices] testContext context)
        {
            if (_db == null)
            {
                _db = context;
            }
        }
        public static Singleton GetInstance(testContext testContext)
        {

            if (instance == null)
            {

                instance = new Singleton(testContext);

            }
            return instance;
        }

        public void SetNull()
        {
            instance = null;
        }
    }


    #endregion

    #region 单例模式2
    public class Singleton2
    {
        private static Singleton2 instance;
        public testContext _db;

        private Singleton2([FromServices] testContext context)
        {
            if (_db == null)
            {
                _db = context;
            }
        }
        public static Singleton2 GetInstance(testContext testContext)
        {

            if (instance == null)
            {

                instance = new Singleton2(testContext);

            }
            return instance;
        }
    }
    #endregion

}