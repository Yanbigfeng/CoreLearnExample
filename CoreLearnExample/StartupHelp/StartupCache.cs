using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using CoreLearnExample.Common;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace CoreLearnExample
{
    public class StartupCache
    {
        public IConfiguration Configuration { get; }
        public StartupCache(IConfiguration configuration)
        {
            Configuration = configuration;

        }



        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {


            #region 添加mvc服务
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            #endregion

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.AddResponseCaching();

            #region 使用redis保存session
            var redisConn = Configuration["Redis:Connection"];
            var redisInstanceName = Configuration["Redis:InstanceName"];
            //使用redis作为分布式缓存
            services.AddDistributedRedisCache(options =>
            {
                options.Configuration = redisConn;
                options.InstanceName = redisInstanceName;
            });
            #endregion
            #region session
            //添加会话服务
            // services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                //设置会话的过期时间
                options.IdleTimeout = TimeSpan.FromSeconds(10);
                options.Cookie.HttpOnly = true;

            });
            #endregion


        }
        //文件存放目录
        // private static readonly string filePath = Assembly.GetEntryAssembly().Location;
        private static readonly string filePath = AppContext.BaseDirectory;
        //文件名称
        private static readonly string fullPath = filePath + "CachePage";
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            #region 环境检测
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            #endregion


            #region 静态文件管道中间件
            app.UseStaticFiles(); //静态文件服务启用

            #endregion


            app.UseResponseCaching();//缓存

            //中间件实现


            CacheMiddleware.CacheMiddlewareMethod(app);

            app.UseCookiePolicy();//启用cookie策略
            app.UseSession();  //添加session


            #region 路由
            //路由
            app.UseMvc(routes =>
            {

                routes.MapRoute(
                         name: "areas",
                     template: "{area:exists}/{controller=Test}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            #endregion
        }
    }
}
