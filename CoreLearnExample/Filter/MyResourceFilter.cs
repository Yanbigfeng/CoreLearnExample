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

        static DomePooledObject demoPolicy = new DomePooledObject();
        static DefaultObjectPool<Dome> defaultPoolWithDemoPolicy = new DefaultObjectPool<Dome>(demoPolicy, 3);

        static List<DefaultObjectPool<Dome>> defaultObjectsList = new List<DefaultObjectPool<Dome>>();
        public void OnResourceExecuting(ResourceExecutingContext context)
        {


            #region list方案未通过
            ///////////////////list方案--未通过
            //var name = context.RouteData.Values["action"].ToString();
            //var c = defaultObjectsList.Where(u => u.Get().id == name).FirstOrDefault();
            //DefaultObjectPool<Dome> resulCabak = null;
            //foreach (var item in defaultObjectsList)
            //{
            //    var d = item;
            //    var namec = item.Get().id;
            //    if (item.Get().id == name)
            //    {
            //        resulCabak = item;
            //    }
            //}
            //if (resulCabak != null)
            //{
            //    var cResult = resulCabak.Get();
            //    context.Result = cResult.view;
            //    c.Return(cResult);
            //} 
            #endregion

            #region 单独实现-通过

            ////////////////////////////////单独实现-通过

            //单个页面测试
            //var Result = defaultPoolWithDemoPolicy.Get();
            //if (Result.id == context.RouteData.Values["action"].ToString())
            //{
            //    context.Result = Result.view;
            //    defaultPoolWithDemoPolicy.Return(Result);
            //}

            #endregion

            /////////////////////////////////继续扩展完善
           
            Dome Result = null;
            for (int i = 0; i < 3; i++)
            {
                Result = defaultPoolWithDemoPolicy.Get();
                if (Result!=null)
                {
                    if (Result.id == context.RouteData.Values["action"].ToString())
                    {
                        context.Result = Result.view;
                        defaultPoolWithDemoPolicy.Return(Result);
                        break;
                    }
                    else
                    {
                        //defaultPoolWithDemoPolicy.Return(Result);
                    }
                }
               

            }


        }

        public void OnResourceExecuted(ResourceExecutedContext context)
        {

            #region list方案-未通过
            ///////////////////list方案--未通过
            //DomePooledObject demoPolicy2 = new DomePooledObject();
            //DefaultObjectPool<Dome> defaultPoolWithDemoPolicy2 = new DefaultObjectPool<Dome>(demoPolicy2, 1);
            //var Result = context.Result as ViewResult;
            ////获取响应体的引用

            //Dome domec = defaultPoolWithDemoPolicy2.Get();
            //Dome dome = new Dome();
            //dome.id = context.RouteData.Values["action"].ToString();
            //dome.nowTime = DateTime.Now.ToString();
            //dome.view = Result;


            //defaultPoolWithDemoPolicy2.Return(dome);
            //defaultObjectsList.Add(defaultPoolWithDemoPolicy2); 
            #endregion


            #region 单页面实现--通过
            ////////////////////////////////单独实现-通过
            //var Result = context.Result as ViewResult;
            ////获取响应体的引用
            //Dome dome = new Dome();
            //dome.id = context.RouteData.Values["action"].ToString();
            //dome.nowTime = DateTime.Now.ToString();
            //dome.view = Result;
            //defaultPoolWithDemoPolicy.Return(dome); 
            #endregion

            /////////////////////////////////继续扩展完善
            //单独实现模型
            var Result = context.Result as ViewResult;
            //获取响应体的引用
            //Dome dome =null;
            for (int i = 0; i < 3; i++)
            {
                Dome dome = defaultPoolWithDemoPolicy.Get();
                if (dome == null)
                {
                    dome = new Dome();
                    dome.id = context.RouteData.Values["action"].ToString();
                    dome.nowTime = DateTime.Now.ToString();
                    dome.view = Result;
                    defaultPoolWithDemoPolicy.Return(dome);
                    break;
                }
                else {

                }


            }
            
        }
    }


    //使用ocjectPool

    public class DomePooledObject : IPooledObjectPolicy<Dome>
    {


        //public DomePooledObject(ObjectResult result) {
        //    _result = result;
        //}
        public Dome Create()
        {
            return null;
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
