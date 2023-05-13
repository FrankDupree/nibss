using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using nibss_orchad_azure.Drivers;
using nibss_orchad_azure.Services;
using OrchardCore.DisplayManagement.Handlers;
using OrchardCore.Users.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using System;
using Microsoft.Extensions.Configuration;
using nibss_orchad_azure.Models;

namespace nibss_orchad_azure
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            //reducing the token lifespan period
            services.Configure<DataProtectionTokenProviderOptions>(o => o.TokenLifespan = TimeSpan.FromMinutes(10));
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(
                options => {
                    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                });
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });
            services.AddAuthorization();
            services.AddScoped<IDisplayDriver<User>, UserProfileDisplayDriver>();
            services.AddOrchardCms().ConfigureServices((tenantServices, serviceProvider) =>{});
            services.AddTransient<TrafService>();

            //Google reCaptcha Service
            services.AddOptions<CaptchaSettings>("Captcha");
            services.AddTransient<CaptchaVerificationService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseForwardedHeaders();
            app.UseExceptionHandler("/Error/500");
            app.UseHsts();
            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");
                context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
                context.Response.Headers.Add("X-Xss-Protection", "1; mode=block");
                await next();
            });
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseOrchardCore(builder => builder
                .UsePoweredByOrchardCore(false)
                .UseCookiePolicy(new CookiePolicyOptions { 
                    HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.Always,
                    Secure = Microsoft.AspNetCore.Http.CookieSecurePolicy.SameAsRequest
                }));
            app.UsePoweredByOrchardCore(false);
        }
    }
}
