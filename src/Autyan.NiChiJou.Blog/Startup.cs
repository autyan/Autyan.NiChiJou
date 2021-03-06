﻿using System.Net;
using Autyan.NiChiJou.Core.Mvc.Attribute;
using Autyan.NiChiJou.Core.Mvc.Extension;
using Autyan.NiChiJou.Core.Mvc.Middleware;
using Autyan.NiChiJou.Model.Extension;
using Autyan.NiChiJou.Repository.Dapper.Extension;
using Autyan.NiChiJou.Service.Blog.Extension;
using Autyan.NiChiJou.UnifyLogin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Autyan.NiChiJou.Blog
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMemoryCache()
                .AddDistributedRedisCache(options =>
                {
                    options.Configuration = Configuration["DistributedCache:Server"];
                    options.InstanceName = Configuration["DistributedCache:Instance"];
                })
                .AddNiChiJouDataModel()
                .AddDapper()
                .UseDapperWithMySql()
                .AddMvcComponent()
                .AddBlogService()
                .AddUnifyLogin(Configuration)
                .AddMvc(options =>
                {
                    var builder = new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .AddAuthenticationSchemes(Configuration["Cookie:Schema"]);
                    options.Filters.Add(new AuthorizeFilter(builder.Build()));
                    options.Filters.Add(new ViewModelValidationActionFilterAttribute());
                    options.Filters.Add(new AjaxRequestActionFilterAttribute());
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                //app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseMiddleware<UnifyExceptionHandlerMiddleware>();
            }

            var forwardedHeadersOptions = new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            };
            foreach (var configurationSection in Configuration.GetSection("Proxies").Value.Split(","))
            {
                if (IPAddress.TryParse(configurationSection, out var addr))
                {
                    forwardedHeadersOptions.KnownProxies.Add(addr);
                }
            }
            app.UseStaticFiles()
                .UseCookiePolicy()
                .UseForwardedHeaders(forwardedHeadersOptions)
                .UseAuthentication()
                .UseMvc(routes =>
                {
                    routes.MapRoute(
                        name: "default",
                        template: "{controller=Home}/{action=Index}/{id?}");
                });
        }
    }
}
