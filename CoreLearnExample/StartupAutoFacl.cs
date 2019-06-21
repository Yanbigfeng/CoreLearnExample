using Autofac;
using Autofac.Extensions.DependencyInjection;
using CoreLearnExample.Lognet;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using CoreLearnExample.Models;
using Microsoft.AspNetCore.Hosting;

namespace CoreLearnExample
{
    /// <summary>
    /// 测试依赖注入替换AutoFac扩展
    /// </summary>

    public class StartupAutoFacl
    {

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            #region 添加mvc服务
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            #endregion

            // 添加 Autofac
            var containerBuilder = new ContainerBuilder();
            //这里是单个注入模型我们可以直接使用内置的注入方式
            // containerBuilder.RegisterModule<DefaultModule>();

            services.AddTransient<ICycleTransient, CycleModel>();//暂时性
            services.AddScoped<ICycleScoped, CycleModel>();//作用域
            services.AddSingleton<ICycleSingleton, CycleModel>();//单例
            services.AddSingleton<ICycleSingletonInstance>(new CycleModel(Guid.Empty));//单例，传参

            // OperationService depends on each of the other Operation types.
            services.AddTransient<CycleService, CycleService>();

            containerBuilder.Populate(services);
            var container = containerBuilder.Build();
            return new AutofacServiceProvider(container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        #region snippet_ConfigureMethod
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
        #endregion
    }


    public class DefaultModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CycleModel>().As<ICycleTransient>();
            builder.RegisterType<CycleModel>().As<ICycleScoped>();
            builder.RegisterType<CycleModel>().As<ICycleSingleton>();
            builder.RegisterType<CycleModel>().As<ICycleSingletonInstance>().UsingConstructor(typeof(Guid));

             builder.RegisterType<CycleService>();

        }
    }
}
