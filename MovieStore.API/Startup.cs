using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MovieStore.Core.RepositoryInterfaces;
using MovieStore.Core.ServiceInterfaces;
using MovieStore.Infrastructure.Data;
using MovieStore.Infrastructure.Repositories;
using MovieStore.Infrastructure.Services;
using Newtonsoft.Json;

namespace MovieStore.API
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
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });
            services.AddDbContext<MovieStoreDbContext>(options =>
              options.UseSqlServer(Configuration.GetConnectionString("MovieStoreDbConnection")));

            services.AddMemoryCache();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                 .AddJwtBearer(options =>
                 {
                     options.TokenValidationParameters = new TokenValidationParameters
                     {
                         ValidateIssuer = false,
                         ValidateAudience = false,
                         ValidateIssuerSigningKey = true,
                         IssuerSigningKey =
                             new SymmetricSecurityKey(Encoding
                                 .UTF8
                                 .GetBytes(Configuration
                                     ["TokenSettings:PrivateKey"]))
                     };
                 });
            services.AddAuthorization(options =>
            {
                var defaultAuthorizationPolicyBuilder =
                    new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme);
                defaultAuthorizationPolicyBuilder = defaultAuthorizationPolicyBuilder.RequireAuthenticatedUser();
                options.DefaultPolicy = defaultAuthorizationPolicyBuilder.Build();
            });

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
                app.UseDeveloperExceptionPage();
            }
            app.UseCors(builder =>
           {
               builder.WithOrigins(Configuration.GetValue<string>("clientSPAUrl")).AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
           });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
