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

            //�������Ҥα��v
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();//�K�[�O�P

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
                app.UseDatabaseErrorPage();//��o�͸�Ʈw���~�ɡA�|��ܬ�������Ʈw���~�T���C
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();//�ҥ� HTTP �Y��ǿ�w�� (HTTP Strict Transport Security�AHSTS)�A�H�W�[���ε{�����w���ʡC�ӳ]�w�b�Ͳ����Ҥ���ĳ�ϥ�
            }
            app.UseHttpsRedirection();//�N HTTP �ШD���s�ɦV�� HTTPS�A�H�T�O�q�T�L�{�����w���ʡC
            app.UseStaticFiles();//�ҥΨϥ��R�A�ɮ� (�p�Ϥ��BJavaScript�BCSS ��) ����O�A�o���ɮץi�H�������s�����s���C

            app.UseCookiePolicy(new CookiePolicyOptions()
            {
                //SameSite �O�@�ئw���ʾ���A�Ω󭭨�󯸽ШD���y���� (Cross-Site Request Forgery�ACSRF)�C
                MinimumSameSitePolicy = SameSiteMode.Lax,
            });

            app.UseRouting();

            app.UseIdentityServer();//�ҥ� IdentityServer�A�Ω󴣨Ѩ������ҩM���v�A�ȡC
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
        /// ���omssql �s�u�r��
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
