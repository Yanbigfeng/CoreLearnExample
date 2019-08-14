using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;


namespace CoreLearnExample.Filter
{
    /// <summary>
    /// 使用结果过滤器格式化返回结果
    /// </summary>
    public class JsonpResultFilter :Attribute,IResultFilter
    {

        //结果返回之前
        public void OnResultExecuting(ResultExecutingContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            string callback = "callback";
            if (IsJsonpRequest(context.HttpContext.Request, out callback))
            {
                //这里可以验证方法名称是否使用了关键字

                //序列化json数据
                ObjectResult result = (ObjectResult)context.Result;
                string json = JsonConvert.SerializeObject(result.Value);
                ObjectResult objectResult = new ObjectResult("/**/ typeof " + callback + " === 'function' && " + callback + "(" + json+");");
                context.Result = objectResult;
            }
            else
            {

            }

        }
        //结果返回之后

        public void OnResultExecuted(ResultExecutedContext context)
        {

        }


        internal static bool IsJsonpRequest(HttpRequest request, out string callback)
        {
            callback = null;

            if (request == null || request.Method.ToUpperInvariant() != "GET")
            {
                return false;
            }

            callback = request.Query
                .Where(kvp => kvp.Key.Equals("callback", StringComparison.OrdinalIgnoreCase))
                .Select(kvp => kvp.Value)
                .FirstOrDefault();

            return !string.IsNullOrEmpty(callback);
        }
    }
}
