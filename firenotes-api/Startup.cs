﻿using AutoMapper;
using dotenv.net;
using firenotes_api.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace firenotes_api
{
    public class Startup
    {
        public static string DatabaseName;
        
        public Startup(IConfiguration configuration)
        {
            DotEnv.Config();
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

            app.Map("/api/notes", AuthenticationMiddleware.ValidateUser);
            
            app.UseMvc();
        }
    }
}