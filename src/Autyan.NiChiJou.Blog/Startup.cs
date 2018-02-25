﻿using Autyan.NiChiJou.Core.Mvc.Attribute;
using Autyan.NiChiJou.Model.Extension;
using Autyan.NiChiJou.Repository.Dapper.Extension;
using Autyan.NiChiJou.UnifyLogin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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
            services.AddMemoryCache()
                .AddDistributedRedisCache(options =>
                {
                    options.Configuration = Configuration["DistributedCache:Server"];
                    options.InstanceName = Configuration["DistributedCache:Instance"];
                })
                .AddNiChiJouDataModel()
                .AddDapper()
                .AddUnifyLogin(Configuration)
                .AddMvc(options =>
                {
                    var builder = new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .AddAuthenticationSchemes(Configuration["Cookie:Schema"]);
                    options.Filters.Add(new AuthorizeFilter(builder.Build()));
                    options.Filters.Add(new ViewModelValidationActionFilterAttribute());
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles()
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
