using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Threading.Tasks;
using idsrv.server.Model;

namespace idsrv.server.Context
{
    public class StoreContext : DbContext
    {
        public StoreContext(DbContextOptions<StoreContext> options) : base(options)
        { }

        public DbSet<ClientModel> Clients { get; set; }
        public DbSet<ApiResourceModel> ApiResources { get; set; }
        public DbSet<IdentityResourceModel> IdentityResources { get; set; }
        public DbSet<UserModel> Users { get; set; }
        public DbSet<SerialModel> Serials { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ClientModel>().HasKey(m => m.ClientId);
            builder.Entity<ApiResourceModel>().HasKey(m => m.ApiResourceName);
            builder.Entity<IdentityResourceModel>().HasKey(m => m.IdentityResourceName);
            builder.Entity<UserModel>().HasKey(m => m.UserName);
            builder.Entity<SerialModel>().HasKey(m => m.Id);

            base.OnModelCreating(builder);
        }

        public async Task Seed()
        {
            if (!await Clients.AnyAsync())
            {
                base.AddRange(Config.GetClients());
                base.AddRange(Config.GetIdentityResources());
                base.AddRange(Config.GetApiResources());
                base.AddRange(Config.GetUsers());
                base.AddRange(Config.GetSerials());
                base.SaveChanges();
            }
        }
    }

    public class ContextFactory : IDesignTimeDbContextFactory<StoreContext>
    {
        public StoreContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<StoreContext>();

            builder.UseSqlite("Data Source=db.identityconfig.sqlite");

            return new StoreContext(builder.Options);
        }
    }
}
