using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLearnExample.Filter
{
    /// <summary>
    /// 使用特性来做权限授权验证
    /// </summary>
    public class MyActionAttribute : ActionFilterAttribute
    {

        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;

        public MyActionAttribute(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            //判断是否存在skip
            var isDefined = false;
            var controllerActionDescriptor = filterContext.ActionDescriptor as ControllerActionDescriptor;
            if (controllerActionDescriptor != null)
            {
                if (IsHaveAllow(controllerActionDescriptor)) return;
            }
            //验证登录

            if (IsLogin())
            {
                //验证通过
            }
            else {

                //验证失败进行跳转
               // filterContext.HttpContext.Response.Redirect("/Login/LogOut");
                filterContext.Result = new RedirectResult("/Login/LogOut");
            }
            base.OnActionExecuting(filterContext);
        }


        #region 判断是否有Skip
        /// <summary>
        /// 判断是否有Skip
        /// </summary>
        /// <param name="filers"></param>
        /// <returns></returns>
        private static bool IsHaveAllow(ControllerActionDescriptor controllerActionDescriptor)
        {
            return controllerActionDescriptor.MethodInfo.GetCustomAttributes(inherit: true)
                     .Any(a => a.GetType().Equals(typeof(SkipAttribute)));

        }
        #endregion


        #region 判断是否登录
        private  bool IsLogin()
        {
            var cc = _httpContextAccessor.HttpContext.Session.GetString("userName");
            //验证登录userName是否为空，如果为空就算作验证失败返回false
            if (!string.IsNullOrEmpty( _httpContextAccessor.HttpContext.Session.GetString("userName")))
            {
                return true;
            }
            return false;
        }
        #endregion

    }
}
