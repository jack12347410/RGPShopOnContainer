using AuthApi.Data;
using AuthApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                
            options.UseSqlServer(MssqlConnStr()));

            //身分驗證及授權
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();//添加令牌

            services.AddControllersWithViews();
            services.AddRazorPages();

            var builder = services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;

                options.EmitStaticAudienceClaim = true;
            })
            .AddDeveloperSigningCredential()
            .AddInMemoryPersistedGrants()
            .AddInMemoryApiScopes(Config.GetApiScopes())
            .AddInMemoryIdentityResources(Config.GetIdentityResources())
            .AddInMemoryApiResources(Config.GetApiResources())
            .AddInMemoryClients(Config.GetClients(Config.GetUrls(Configuration)))
            .AddAspNetIdentity<ApplicationUser>();


            //services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddControllersWithViews();
            services.AddAuthentication();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseMigrationsEndPoint();
                app.UseDatabaseErrorPage();//當發生資料庫錯誤時，會顯示相關的資料庫錯誤訊息。
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();//啟用 HTTP 嚴格傳輸安全 (HTTP Strict Transport Security，HSTS)，以增加應用程式的安全性。該設定在生產環境中建議使用
            }
            app.UseHttpsRedirection();//將 HTTP 請求重新導向至 HTTPS，以確保通訊過程中的安全性。
            app.UseStaticFiles();//啟用使用靜態檔案 (如圖片、JavaScript、CSS 等) 的能力，這些檔案可以直接由瀏覽器存取。

            app.UseCookiePolicy(new CookiePolicyOptions()
            {
                //SameSite 是一種安全性機制，用於限制跨站請求偽造攻擊 (Cross-Site Request Forgery，CSRF)。
                MinimumSameSitePolicy = SameSiteMode.Lax,
            });

            app.UseRouting();

            app.UseIdentityServer();//啟用 IdentityServer，用於提供身份驗證和授權服務。
            app.UseAuthentication();
            app.UseAuthorization();

            app.Use(async (conext, next) =>
            {
                await next();
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }

        /// <summary>
        /// 取得mssql 連線字串
        /// </summary>
        /// <returns></returns>
        private string MssqlConnStr()
        {
            string server = Configuration["DatabaseServer"];
            string database = Configuration["DatabaseName"];
            string user = Configuration["DatabaseUser"];
            string password = Configuration["DatabasePassword"];
            string connectionString = string.Format("Server={0};Database={1};User={2};Password={3};", server, database, user, password);

            return connectionString;
        }
    }
}
