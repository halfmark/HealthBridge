using HealthBridge.BL.Interfaces;
using HealthBridge.BL.Services;
using HealthBridge.External.Service.Implementation;
using HealthBridge.External.Service.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HealthBridge.RapidApi
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
            //Dependency injection register services
            services.AddTransient<ICovidService, CovidService>();
            services.AddTransient<IRapidApiService, RapidApiService>();

            services.AddControllers();
            //All api versioning
            services.AddApiVersioning();
            // Register the Swagger generator, defining 1 or more Swagger documents  
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Covid-19 API",
                    Description = "Covid-19 API - .NET Core API",

                    Contact = new OpenApiContact
                    {
                        Name = "Thabang Motimele",
                        Email = string.Empty

                    }

                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            var path = Directory.GetCurrentDirectory();

            loggerFactory.AddFile($"{path}\\Logs\\Log.txt");

            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
 
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.DocumentTitle = "Swagger UI - Covid-19 Api";
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Covid-19 Api v1");

            });

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
