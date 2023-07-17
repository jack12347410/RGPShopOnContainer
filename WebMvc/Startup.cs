using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WebMvc.Infrastructure;
using WebMvc.Services;

namespace WebMvc
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
            services.Configure<AppSettings>(Configuration);
            services.AddHttpClient();
            services.AddSingleton<IHttpClient, CustomHttpClient>();
            services.AddTransient<ICatalogService, CatalogService>();

            services.AddControllersWithViews();

            /*使用Cookie驗證方案和OpenID Connect驗證方案來實現使用者身份驗證和授權，與指定的授權伺服器進行交互，並獲取必要的令牌和聲明資訊
             -----------------------------------------------------------------------------------------------------------------------*/
            JwtSecurityTokenHandler.DefaultMapInboundClaims = false;//不會自動將入站的聲明映射到用戶身份對像中
            services.AddAuthentication(option =>
            {
                option.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;//默認的身份驗證
                option.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;//默認的挑戰
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)//Cookie認證
            .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, option =>
            {
                option.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;//使用的登錄方案，這裡使用了Cookie驗證方案。
                option.Authority = Configuration.GetValue<string>("IdentityUrl");//OpenID Connect授權伺服器的URL
                option.SignedOutRedirectUri = Configuration.GetValue<string>("CallBackUrl");//使用者登出後的重新導向URL
                option.ClientId = "mvc"; //客戶端的識別和密鑰，用於向授權伺服器進行身份驗證。
                option.ClientSecret = "secret";
                option.ResponseType = "code id_token";//授權伺服器的回應類型，這裡使用了授權碼和ID令牌
                option.SaveTokens = true;//是否保存接收到的令牌
                option.GetClaimsFromUserInfoEndpoint = true;//是否從使用者資訊端點獲取聲明
                option.RequireHttpsMetadata = false;//是否要求使用HTTPS連線與授權伺服器進行通訊
                option.Scope.Add("openid");
                option.Scope.Add("profile");
                option.Scope.Add("offline_access");
                option.NonceCookie.SameSite = SameSiteMode.Lax;//設置Cookie的SameSite屬性為Lax，以改善跨站請求偽造（CSRF）的安全性
                option.CorrelationCookie.SameSite = SameSiteMode.Lax;
                option.BackchannelHttpHandler = new HttpClientHandler()
                {
                    //設置用於發出後台HTTP請求的自訂HttpClientHandler
                    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator//允許接受任何伺服器憑證
                };

                //option.Events = new OpenIdConnectEvents()
                //{
                //    //修複登錄成功之後轉向signin-oidc並報錯的問題
                //    OnRemoteFailure = ctx =>
                //    {
                //        ctx.Response.Redirect($"{ctx.Request.Scheme}://{ctx.Request.Host}/Catalog/About");
                //        ctx.HandleResponse();
                //        //ctx.Response.Body.WriteAsync(null);
                //        return Task.CompletedTask;
                //    }
                //};
            });
            /*------------------------------------------------------------------------------------------------------------------------------*/
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Catalog}/{action=Index}/{id?}");
            });
        }
    }
}
