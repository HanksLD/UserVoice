using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserVoice.Repository;
using UserVoice.Entity.IRepositories;
using UserVoice.Service;
using UserVoice.Application;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace UserVoice.WebApp
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
            //DbContext
            services.AddDbContext<BoardMessageDbContext>((provider, builder) =>
            {
                builder.UseMySql(Configuration.GetConnectionString("DefaultConnection"), b =>
                {
                    b.CommandTimeout(10);
                });
            });
            services.AddLogging();
            services.Add(new ServiceDescriptor(typeof(DbContext), typeof(BoardMessageDbContext), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(ICategoryRepository), typeof(CategoryRepository), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IMsgArticleRepository), typeof(MsgArticleRepository), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IAppUserRepository), typeof(AppUserRepository),ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(ICategoryService), typeof(CategoryService), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IMsgArticleService), typeof(MsgArticleService), ServiceLifetime.Scoped));
            services.Add(new ServiceDescriptor(typeof(IAppUserService), typeof(AppUserService), ServiceLifetime.Scoped));
            //https://www.cnblogs.com/oorz/p/8617530.html cookie授权登录参考
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,options =>
                {
                    options.Cookie.HttpOnly = true;
                    options.Cookie.Expiration = TimeSpan.FromMinutes(30);
                    // If the LoginPath isn't set, ASP.NET Core defaults 
                    // the path to /Account/Login.
                    options.LoginPath = "/Account/Login";
                    options.Cookie.Name = CookieAuthenticationDefaults.AuthenticationScheme;
                });
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
