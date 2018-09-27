﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Yunify.Auth.Api.Client;
using Yunify.Auth.Api.Config;
using Yunify.Auth.Api.Filters;

namespace Yunify.Auth.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options => options.Filters.Add<ExceptionFilter>());

            var authClientSection = Configuration.GetSection("AuthClient");
            AuthClientConfig authClientConfig = authClientSection.Get<AuthClientConfig>();
            services.Configure<AuthClientConfig>(authClientSection);

            services.AddSingleton<IAuthClient, AuthClient>();

            services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = authClientConfig.Authority;
                    options.RequireHttpsMetadata = false;
                    options.ApiSecret = authClientConfig.ClientSecret;
                    options.ApiName = authClientConfig.DefaultScope;
                });

            services.AddMemoryCache();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();
            app.UseStaticFiles();
            app.UseMvc();
        }
    }
}
