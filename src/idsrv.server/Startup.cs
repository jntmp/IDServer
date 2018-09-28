using IdentityServer4.Stores;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using idsrv.server.Context;
using idsrv.server.Model;
using idsrv.server.Store;
using idsrv.server.Store.Context;
using idsrv.server.Store.User;

namespace idsrv.server
{
    public class Startup
    {
        IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration) => Configuration = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            string connString = Configuration.GetConnectionString("StoreConnection");
             
            services.AddMvc();

            services
                .AddDbContext<StoreContext>(options =>
                    options.UseSqlite(connString, b => b.MigrationsAssembly("idsrv.server"))
                );

            services.AddTransient<IClientStore, ClientStore>();
            services.AddTransient<IResourceStore, ResourceStore>();
            services.AddTransient<ISerialStore, SerialStore>();

            services
                .AddIdentity<UserModel, UserRole>()
                //.AddEntityFrameworkStores<UserDbContext>()
                //.AddEntityFrameworkStores<ConfigurationStoreContext>()
                .AddUserStore<UserStore>()
                .AddDefaultTokenProviders();

            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddResourceStore<ResourceStore>()
                .AddClientStore<ClientStore>()
                .AddResourceOwnerValidator<UserStore>();

            services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication("token", options =>
                {
                    options.Authority = "http://localhost:5000/";
                    options.RequireHttpsMetadata = false;
                    options.ApiName = "api1";
                });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, StoreContext configDbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            configDbContext.Seed().Wait();

            app.UseIdentityServer();
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
