using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MovieStore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using MovieStore.Core.RepositoryInterfaces;
using MovieStore.Core.ServiceInterfaces;
using MovieStore.Infrastructure.Services;
using MovieStore.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.CodeAnalysis.Options;
using MovieStore.MVC.Helpers;

namespace MovieStore.MVC
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
            services.AddControllersWithViews();

            services.AddDbContext<MovieStoreDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("MovieStoreDbConnection")));

            services.AddMemoryCache();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie
                (
                    options =>
                    {
                        options.Cookie.Name = "MovieStoreAuthCookie";
                        options.ExpireTimeSpan = TimeSpan.FromHours(2);
                        options.LoginPath = "/Account/Login";
                    }
                );

            // DI in ASP.NET Core has 3 types of Lifetimes, Scoped, Singleton, Transient
            services.AddScoped<IMovieRepository, MovieRepository>();
            services.AddScoped<IMovieService, MovieService>();
            services.AddScoped<IGenreRepository, GenreRepository>();
            services.AddScoped<IGenreService, GenreService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICryptoService, CryptoService>();
            services.AddScoped<IPurchaseRepository, PurchaseRepository>();
            services.AddScoped<IReviewRepository, ReviewRepository>();
            services.AddScoped<IFavoriteRepository, FavoriteRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                 //app.UseDeveloperExceptionPage();
                app.UseMovieStoreExceptionMiddleware();
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
                // Routing -- Pattern matching technique --check if is match
               // 1. Tarditional way of routing
               // 2. Attribute based Routing
                // http:localhost:2222/Movies/index/3
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
