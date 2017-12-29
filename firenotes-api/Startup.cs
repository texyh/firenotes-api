﻿using System.IO;
using AutoMapper;
using dotenv.net;
using firenotes_api.Configuration;
using firenotes_api.Interfaces;
using firenotes_api.Middleware;
using firenotes_api.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace firenotes_api
{
    public class Startup
    {
        public static string DatabaseName;
        
        public Startup(IConfiguration configuration)
        {
            string fullPath = Path.GetFullPath(".env");
            DotEnv.Config(false, fullPath);
            
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options => options.AddPolicy("CorsPolicy", builder =>
            {
                builder
                    .AllowAnyMethod()
                    .AllowAnyOrigin()
                    .AllowCredentials()
                    .AllowAnyHeader();
            }))
            services.AddAutoMapper();
            services.AddMvc();

            // register the services
            services.AddScoped<INoteService, NoteService>();
            services.AddScoped<IUserService, UserService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            DatabaseName = "firenotes-prod-db";
            
            if (env.IsDevelopment())
            {
                DatabaseName = "firenotes-dev-db";
                app.UseDeveloperExceptionPage();
            }

            if (env.IsEnvironment("test"))
            {
                DatabaseName = "firenotes-test-db";
            }

            app.UseCors("CorsPolicy");

            app.UseAuthenticationMiddleware();
            
            app.UseMvc();
        }
    }
}