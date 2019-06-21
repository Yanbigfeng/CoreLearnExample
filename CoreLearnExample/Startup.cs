using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using log4net;
using log4net.Config;
using log4net.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using CoreLearnExample.Lognet;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Server.IIS;
using CoreLearnExample.Models;
using CoreLearnExample.EData;
using Microsoft.EntityFrameworkCore;

namespace CoreLearnExample
{
    public class Startup
    {
        //启动是为了访问配置文件定义的变量
        public IConfiguration Configuration { get; }
        //日志需要指定
        public static ILoggerRepository repository { get; set; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            //加载log4net日志配置文件
            repository = LogManager.CreateRepository("NETCoreRepository");
            XmlConfigurator.Configure(repository, new FileInfo("log4net.config"));

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

            #region 添加mvc服务
            services.AddMvc(options =>
              {
                  options.Filters.Add<HttpGlobalExceptionFilter>(); //加入全局异常类

              }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            #endregion


            #region appsettings文件的读取
            //在启动时候访问配置文件
            var appValue = Configuration.GetValue<string>("AppSetValue", "");
            //绑定到类
            PeserModel peserModel = new PeserModel();
            Configuration.GetSection("PeserModel").Bind(peserModel);

            //多层类嵌套的绑定
            AppSetModel appSetModel = new AppSetModel();
            Configuration.GetSection("AppSetModel").Bind(appSetModel);
            AppSetModel setModel = Configuration.GetSection("AppSetModel").Get<AppSetModel>();
            //读取数组
            AppSetArrayModel setArrayModel = new AppSetArrayModel();
            Configuration.GetSection("AppSetArrayModel").Bind(setArrayModel);
            #endregion

            #region 目录浏览
            services.AddDirectoryBrowser();   //添加目录浏览服务
                                              //IIS选项设置进程内置承载
                                              //services.Configure<IISServerOptions>(options =>
                                              //{
                                              //    options.AutomaticAuthentication = false;
                                              //});

            #endregion

            #region EF注册上下文
            //通过注册使用上下文
            //var connection = @"Server=YBF;Database=test;Trusted_Connection=True;";
            //services.AddDbContext<TestContext>(options => options.UseSqlServer(connection));

            services.AddDbContext<testContext>(options =>
                     options.UseSqlServer(Configuration.GetConnectionString("TestEntity")));
            #endregion

            #region 依赖注入的生命周期【内置】
            services.AddTransient<ICycleTransient,CycleModel>();//暂时性
            services.AddScoped<ICycleScoped, CycleModel>();//作用域
            services.AddSingleton<ICycleSingleton, CycleModel>();//单例
            services.AddSingleton<ICycleSingletonInstance>(new CycleModel(Guid.Empty));//单例，传参

            // OperationService depends on each of the other Operation types.
            services.AddTransient<CycleService, CycleService>();
            #endregion


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

            app.UseHttpsRedirection();//重定向

            #region 静态文件管道中间件
            app.UseStaticFiles(); //静态文件服务启用
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), "upload")), //文件的实际路径位置
                RequestPath = "/StaticFiles",//这里是设置访问的时候路径名称可以任意定义
            });
            #endregion

            #region 启用目录浏览
            //启用目录浏览
            app.UseDirectoryBrowser(new DirectoryBrowserOptions
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), "upload", "img")),
                RequestPath = "/StaticFiles/img"
            });
            #endregion

            app.UseCookiePolicy();
            app.UseSession();  //添加session


            #region 路由
            //路由
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            #endregion
        }
    }
}
