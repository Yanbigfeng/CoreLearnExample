using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.ObjectPool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLearnExample.Filter
{
    /// <summary>
    /// 资源过滤器+ObjectPooled做页面缓存
    /// </summary>
    public class MyResourceFilter : Attribute, IResourceFilter
    {

     static   DomePooledObject demoPolicy = new DomePooledObject();
      static  DefaultObjectPool<Dome> defaultPoolWithDemoPolicy = new DefaultObjectPool<Dome>(demoPolicy, 1);
        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            var Result2 = defaultPoolWithDemoPolicy.Get();
            //获取响应体的引用

            //这里设置文件的缓存时间值
            var hasExpire = int.TryParse("10", out var expire);
            if (hasExpire && expire > 0)
            {

            }
            if (Result2.id != "1")
            {
                context.Result = Result2.view;
                defaultPoolWithDemoPolicy.Return(Result2);
            }

        }

        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            var Result = context.Result as ViewResult;
            //获取响应体的引用
            Dome dome = new Dome();
            dome.id = "2";
            dome.nowTime = DateTime.Now.ToString();
            dome.view = Result;
            defaultPoolWithDemoPolicy.Return(dome);
        }
    }


    //使用ocjectPool

    public class DomePooledObject : IPooledObjectPolicy<Dome>
    {

        ObjectResult _result = new ObjectResult(1);
        //public DomePooledObject(ObjectResult result) {
        //    _result = result;
        //}
        public Dome Create()
        {
            return new Dome()
            {
                id = "1",
                nowTime = DateTime.Now.ToString()
            };
        }

        public bool Return(Dome obj)
        {
            return true;
        }
    }

    //帮助类
    public class Dome
    {

        public string id { get; set; }
        public string nowTime { get; set; }

        public ViewResult view { get; set; }

    }
}
