using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AccountOwnerServer.Extensions;
 
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;

namespace HillYatraAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            LogManager.LoadConfiguration(String.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureCors();
            services.ConfigureIISIntegration();
            services.ConfigureMySqlContext(Configuration);
            //services.AddCors(options =>
            //{
            //    options.AddPolicy(
            //      "CorsPolicy",
            //      builder => builder.WithOrigins("http://localhost:4200")
            //      .AllowAnyMethod()
            //      .AllowAnyHeader()
            //      .AllowCredentials());
            //});

            //services.AddDbContext<RepositoryContext>(opt => opt.UseSqlServer(Configuration.GetConnectionString("Data:DefaultConnection")));
            //services.AddEntityFrameworkSqlServer().AddDbContext<RepositoryContext>();
            services.ConfigureLoggerService(); 
            services.ConfigureRepositoryWrapper();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            

            app.UseStaticFiles();

           

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                // ForwardedHeaders = ForwardedHeaders.All

                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto

            });
            app.UseCors("AllowOrigin");
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
        
            app.UseHttpsRedirection();
       


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
