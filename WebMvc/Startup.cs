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

            /*�ϥ�Cookie���Ҥ�שMOpenID Connect���Ҥ�רӹ�{�ϥΪ̨������ҩM���v�A�P���w�����v���A���i��椬�A��������n���O�P�M�n����T
             -----------------------------------------------------------------------------------------------------------------------*/
            JwtSecurityTokenHandler.DefaultMapInboundClaims = false;//���|�۰ʱN�J�����n���M�g��Τᨭ���ﹳ��
            services.AddAuthentication(option =>
            {
                option.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;//�q�{����������
                option.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;//�q�{���D��
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)//Cookie�{��
            .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, option =>
            {
                option.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;//�ϥΪ��n����סA�o�̨ϥΤFCookie���Ҥ�סC
                option.Authority = Configuration.GetValue<string>("IdentityUrl");//OpenID Connect���v���A����URL
                option.SignedOutRedirectUri = Configuration.GetValue<string>("CallBackUrl");//�ϥΪ̵n�X�᪺���s�ɦVURL
                option.ClientId = "mvc"; //�Ȥ�ݪ��ѧO�M�K�_�A�Ω�V���v���A���i�樭�����ҡC
                option.ClientSecret = "secret";
                option.ResponseType = "code id_token";//���v���A�����^�������A�o�̨ϥΤF���v�X�MID�O�P
                option.SaveTokens = true;//�O�_�O�s�����쪺�O�P
                option.GetClaimsFromUserInfoEndpoint = true;//�O�_�q�ϥΪ̸�T���I����n��
                option.RequireHttpsMetadata = false;//�O�_�n�D�ϥ�HTTPS�s�u�P���v���A���i��q�T
                option.Scope.Add("openid");
                option.Scope.Add("profile");
                option.Scope.Add("offline_access");
                option.NonceCookie.SameSite = SameSiteMode.Lax;//�]�mCookie��SameSite�ݩʬ�Lax�A�H�ﵽ�󯸽ШD���y�]CSRF�^���w����
                option.CorrelationCookie.SameSite = SameSiteMode.Lax;
                option.BackchannelHttpHandler = new HttpClientHandler()
                {
                    //�]�m�Ω�o�X��xHTTP�ШD���ۭqHttpClientHandler
                    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator//���\����������A������
                };

                //option.Events = new OpenIdConnectEvents()
                //{
                //    //�׽Ƶn�����\������Vsignin-oidc�ó��������D
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
