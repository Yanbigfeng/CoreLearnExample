using CoreLearnExample.Filter;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreLearnExample.ViewModels;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CoreLearnExample
{
    public class StartupFilter
    {

        static ISession session;

        public IConfiguration Configuration { get; }
        public StartupFilter(IConfiguration configuration)
        {
            Configuration = configuration;

        }



        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            #region session
            //添加会话服务
            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                //设置会话的过期时间
                options.IdleTimeout = TimeSpan.FromSeconds(10);
                options.Cookie.HttpOnly = true;
                // cookie是必须的
                options.Cookie.IsEssential = true;
            });
            #endregion

            // services.AddScoped<MyAuthorizeFilter>();
            #region 添加mvc服务
            services.AddMvc(options =>
            {

                // options.Filters.Add(new MyAuthorizeFilter());
                 options.Filters.Add<MyAuthorizeFilter>(); // 添加身份验证过滤器
               // options.Filters.Add<MyActionAttribute>(); // 使用的是操作action筛选器---已测试通过
                options.AllowCombiningAuthorizeFilters = true;
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            #endregion

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IActionFilter, MyActionAttribute>();//构造函数注入
            services.AddScoped<IAuthorizationFilter, MyAuthorizeFilter>();//构造函数注入
        }

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

            app.UseStaticFiles(); //静态文件服务启用
            app.UseSession();  //添加session

            #region 路由
            //路由
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Login}/{action=Login}/{id?}");
            });

            #endregion
        }
    }
}
