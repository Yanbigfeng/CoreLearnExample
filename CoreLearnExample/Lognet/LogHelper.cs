using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLearnExample.Lognet
{
    public class LogHelper
    {
        private static ILog myLog = LogManager.GetLogger(StartupMvcBase.repository.Name, typeof(LogHelper));

        /// <summary>
        /// 记录日志信息
        /// </summary>
        /// <param name="msg"></param>
        public static void Info(string msg)
        {
            myLog.Info(msg);
        }
        /// <summary>
        /// 记录错误信息
        /// </summary>
        /// <param name="msg"></param>
        public static void Error(string msg)
        {
            myLog.Error(msg);
        }
        /// <summary>
        /// 记录错误信息及异常对象
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="ex"></param>
        public static void Error(string msg, Exception ex)
        {
            myLog.Error(msg, ex);
        }

    }
}
