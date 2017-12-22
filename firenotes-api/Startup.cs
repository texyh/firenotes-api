﻿using System;
using System.IO;
using AutoMapper;
using dotenv.net;
using firenotes_api.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace firenotes_api
{
    public class Startup
    {
        public static string DatabaseName;
        
        public Startup(IConfiguration configuration)
        {
            string fullPath = Path.GetFullPath(".env");
            Console.WriteLine(fullPath);
            DotEnv.Config(true, fullPath);
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper();
            services.AddMvc();
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

            app.UseAuthenticationMiddleware();
            
            app.UseMvc();
        }
    }
}