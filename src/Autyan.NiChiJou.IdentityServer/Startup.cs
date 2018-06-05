using Autyan.NiChiJou.Core.Mvc.Attribute;
using Autyan.NiChiJou.Core.Mvc.Authorization;
using Autyan.NiChiJou.Core.Mvc.Extension;
using Autyan.NiChiJou.IdentityServer.Consts;
using Autyan.NiChiJou.Model.Extension;
using Autyan.NiChiJou.Repository.Dapper.Extension;
using Autyan.NiChiJou.Service.Identity.Extension;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Autyan.NiChiJou.IdentityServer
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

            services.AddDistributedRedisCache(options =>
                {
                    options.Configuration = Configuration["DistributedCache:Server"];
                    options.InstanceName = Configuration["DistributedCache:Instance"];
                })
                .AddIdentityCache(Configuration)
                .AddNiChiJouDataModel()
                .AddDapper()
                .UseDapperWithMySql()
                .AddIdentityService()
                .AddMvcComponent()
                .AddScoped<IServiceTokenProvider, ServiceTokenProvider>()
                .AddScoped<SignInManager, SignInManager>()
                .AddAuthentication(options => options.DefaultScheme = Configuration["Cookie:Schema"])
                .AddCookieAuthentication(Configuration)
                .AddServiceTokenAuthentication(Configuration)
                .Services
                .AddMvc(options =>
                {
                    var builder = new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .AddAuthenticationSchemes(Configuration["Cookie:Schema"])
                        .AddAuthenticationSchemes(Configuration["ServiceToken:Schema"]);
                    options.Filters.Add(new AuthorizeFilter(builder.Build()));
                    options.Filters.Add(new ViewModelValidationActionFilterAttribute());
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1).Services
                .AddAuthorization(options =>
                {
                    options.AddPolicy(AuthorizePolicy.InternalServiceOnly, policy => policy.RequireClaim(Configuration["ServiceToken:Schema"]));
                });
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
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseStaticFiles()
                .UseCookiePolicy()
                .UseForwardedHeaders(new ForwardedHeadersOptions
                {
                    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
                })
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