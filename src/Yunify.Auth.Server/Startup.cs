using IdentityServer4.Stores;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Yunify.Auth.Server.Context;
using Yunify.Auth.Server.Model;
using Yunify.Auth.Server.Store;
using Yunify.Auth.Server.Store.Context;
using Yunify.Auth.Server.Store.User;

namespace Yunify.Auth.Server
{
    public class Startup
    {
        IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration) => Configuration = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            string connString = Configuration.GetConnectionString("ConfigurationStoreConnection");

            services.AddMvc();

            services
                .AddDbContext<ConfigurationStoreContext>(options =>
                    options.UseSqlite(connString, b => b.MigrationsAssembly("Yunify.Auth.Server"))
                )
                .AddDbContext<UserDbContext>(options =>
                    options.UseSqlite(connString, b => b.MigrationsAssembly("Yunify.Auth.Server"))
                );

            services
                .AddIdentity<UserModel, UserRole>()
                .AddEntityFrameworkStores<UserDbContext>()
                .AddUserStore<UserStore>()
                .AddDefaultTokenProviders();

            services.AddTransient<IClientStore, ClientStore>();
            services.AddTransient<IResourceStore, ResourceStore>();
            services.AddTransient<ISerialStore, SerialStore>();
            
            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddResourceStore<ResourceStore>()
                .AddClientStore<ClientStore>()
                .AddAspNetIdentity<UserModel>();

            services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication("token", options =>
                {
                    options.Authority = "http://localhost:5000/";
                    options.RequireHttpsMetadata = false;
                    options.ApiName = "api1";
                });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ConfigurationStoreContext configDbContext, UserDbContext userDbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            configDbContext.Seed().Wait();
            userDbContext.Seed().Wait();

            app.UseIdentityServer();
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
