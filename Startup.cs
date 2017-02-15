using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OwnAspNetCore.Infra;
using OwnAspNetCore.Services;

namespace OwnAspNetCore
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ISecurity>(new SecurityProvider());
            services.AddSingleton<ISettings>(SettingsProvider.LoadSettings());
            services.AddSingleton<IDatabase>(new LiteDbProvider());

            //Set Admin policy
            services.AddAuthorization(o =>
           {
               o.AddPolicy(UserRoles.Admin, p => p.AddRequirements(new HasRoleRequirement(UserRoles.Admin)));
           });

            //Set MVC with default lockdown
            services.AddMvc(
                config =>
                {
                    var policy = new AuthorizationPolicyBuilder()
                                     .RequireAuthenticatedUser()
                                     .Build();
                    config.Filters.Add(new AuthorizeFilter(policy));
                }
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCookieAuthentication(new CookieAuthenticationOptions()
            {
                AuthenticationScheme = "CookieAuth",
                LoginPath = new PathString("/Login"),
                AccessDeniedPath = new PathString("/Forbidden"),
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
            });

            app.UseStaticFiles();

            app.UseMvc(routes =>
                {
                    routes.MapRoute(
                        name: "default",
                        template: "{controller=Home}/{action=Index}");
                }
            );
        }
    }
}
