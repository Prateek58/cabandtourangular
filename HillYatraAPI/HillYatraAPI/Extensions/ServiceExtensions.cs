
using Contracts;
using Google.Protobuf.WellKnownTypes;
using HillYatraAPI.Models;
using LoggerService;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Repository;

namespace AccountOwnerServer.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowOrigin", builder =>
             builder.WithOrigins("https://cabapp.apps.prateekbhardwaj.dev")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
            });

            //services.AddCors(options =>
            //{
            //    options.AddPolicy("CorsPolicy",
            //        builder => builder.WithOrigins("https://cabapp.apps.prateekbhardwaj.dev",
            //                                      "https://api.cabapp.apps.prateekbhardwaj.dev")
            //                                      .AllowAnyHeader()
            //                                      .AllowAnyMethod());
            //});


        }

        public static void ConfigureIISIntegration(this IServiceCollection services)
        {
            services.Configure<IISOptions>(options =>
            {

            });
        }

        public static void ConfigureLoggerService(this IServiceCollection services)
        {
            services.AddSingleton<ILoggerManager, LoggerManager>();
        }
        public static void ConfigureMySqlContext(this IServiceCollection services, IConfiguration config)
        {
            var connectionString = config["mysqlconnection:connectionString"];
           // var connectionString = config["mssqlconnectiongcp:connectionString"];
            services.AddDbContext<RepositoryContext>(o => o.UseMySql(connectionString));
           // services.AddDbContext<RepositoryContext>(o => o.UseSqlServer(connectionString));
          //  services.AddDbContext<RepositoryContext>(o => o.UseSqlServer(connectionString));
        }
        public static void ConfigureRepositoryWrapper(this IServiceCollection services)
        {
            services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
        }

    }
}
