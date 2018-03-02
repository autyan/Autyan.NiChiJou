using System;
using System.Threading.Tasks;
using Autyan.NiChiJou.Core.Options;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Autyan.NiChiJou.UnifyLogin
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddUnifyLogin(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<LoginApiManager, LoginApiManager>()
                .AddSingleton<Passport, Passport>()
                .AddAuthentication(options => options.DefaultScheme = configuration["Cookie:Schema"])
                .AddCookie(configuration["Cookie:Schema"], options =>
                {
                    options.Events = new CookieAuthenticationEvents
                    {
                        OnRedirectToLogin = async (context) => await Task.Factory.StartNew(() => context.HttpContext.Response.Redirect(configuration["Cookie:LoginPath"])),
                        OnRedirectToLogout = async (context) => await Task.Factory.StartNew(() => context.HttpContext.Response.Redirect(configuration["Cookie:LogoutPath"]))
                    };
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(double.Parse(configuration["Cookie:Expiration"]));
                }).Services
            .AddOptions()
            .Configure<UnifyLoginOptions>(configuration.GetSection(nameof(UnifyLogin)))
            .Configure<AutyanCookieOptions>(configuration.GetSection("Cookie"));
            return services;
        }
    }
}
