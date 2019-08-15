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
    /// 授权过滤器(登录验证)
    /// </summary>
    public class MyAuthorizeFilter :Attribute, IAuthorizationFilter
    {

        private readonly IHttpContextAccessor _httpContextAccessor;

        public MyAuthorizeFilter()
        {
        }
        public MyAuthorizeFilter(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {

            //判断是否存在skip
            var controllerActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            var fil = context.Filters;
            if (IsHaveAllow(context.Filters))
            {
                return;
            }

            //验证登录
            if (IsLogin())
            {

            }
            else
            {
                context.Result = new RedirectResult("/Login/LogOut");
            }


        }


        #region 全局拦截器

        //public override async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        //{
        //    //判断是否存在skip
        //    if (IsHaveAllow(context.Filters))
        //    {
        //        return;
        //    }

        //    //验证登录
        //    if (IsLogin())
        //    {

        //    }
        //    else {
        //        context.Result = new RedirectResult("/Login/LogOut");
        //    }



        //}
        #endregion

        #region 判断是否有Skip

        /// <summary>
        /// 判断是否有Skip
        /// </summary>
        /// <param name="filers"></param>
        /// <returns></returns>
        private static bool IsHaveAllow(IList<IFilterMetadata> filers)
        {
            for (int i = 0; i < filers.Count; i++)
            {
                if (filers[i] is SkipAttribute)
                {
                    return true;
                }
            }
            return false;

        }
        #endregion


        #region 判断是否登录
        private bool IsLogin()
        {
            //验证登录userName是否为空，如果为空就算作验证失败返回false
            if (!string.IsNullOrEmpty(_httpContextAccessor.HttpContext.Session.GetString("userName")))
            {
                return true;
            }
            return false;
        }
        #endregion
    }
}
