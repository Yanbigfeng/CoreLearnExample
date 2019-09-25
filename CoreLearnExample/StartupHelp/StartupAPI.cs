using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using WebApiContrib.Core.Formatter.Jsonp;
using CoreLearnExample.Util;
using CoreLearnExample.Filter;

namespace CoreLearnExample
{
    public class StartupAPI
    {

        public IConfiguration Configuration { get; }
        public StartupAPI(IConfiguration configuration)
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

            //添加缓存
            services.AddMemoryCache();

            //配置cors
            //services.AddCors(option => option.AddPolicy("cors", policy => policy.AllowAnyHeader().AllowAnyMethod().AllowCredentials().AllowAnyOrigin()));
            services.AddCors(option => option.AddPolicy("cors", policy => policy.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins(new[] { "http://127.0.0.1:8848" })));


            services.AddScoped<JsonpResultFilter>();

            #region 添加mvc服务

            services.AddMvc(options =>
            {
                // options.InputFormatters.Insert(0, new JsonpMediaTypeFormatter(outputFormatter, null));
                // options.OutputFormatters.Insert(0, new JsonpFormatter(options.OutputFormatters.OfType<JsonOutputFormatter>().FirstOrDefault(), null));
                // options.AddJsonpOutputFormatter(); 
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
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

            app.UseStaticFiles(); //静态文件服务启用


            app.UseCors("cors");//添加cors
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
