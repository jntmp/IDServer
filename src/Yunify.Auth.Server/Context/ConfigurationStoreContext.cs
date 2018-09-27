using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Threading.Tasks;
using Yunify.Auth.Server.Model;

namespace Yunify.Auth.Server.Context
{
    public class ConfigurationStoreContext : DbContext
    {
        public ConfigurationStoreContext(DbContextOptions<ConfigurationStoreContext> options) : base(options)
        { }

        public DbSet<ClientModel> Clients { get; set; }
        public DbSet<ApiResourceModel> ApiResources { get; set; }
        public DbSet<IdentityResourceModel> IdentityResources { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ClientModel>().HasKey(m => m.ClientId);
            builder.Entity<ApiResourceModel>().HasKey(m => m.ApiResourceName);
            builder.Entity<IdentityResourceModel>().HasKey(m => m.IdentityResourceName);

            base.OnModelCreating(builder);
        }

        public async Task Seed()
        {
            if (!await Clients.AnyAsync())
            {
                base.AddRange(Config.GetClients());
                base.AddRange(Config.GetIdentityResources());
                base.AddRange(Config.GetApiResources());
                base.SaveChanges();
            }
        }
    }

    public class ConfigurationStoreContextFactory : IDesignTimeDbContextFactory<ConfigurationStoreContext>
    {
        public ConfigurationStoreContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<ConfigurationStoreContext>();

            builder.UseSqlite("Data Source=db.identityconfig.sqlite");

            return new ConfigurationStoreContext(builder.Options);
        }
    }
}
