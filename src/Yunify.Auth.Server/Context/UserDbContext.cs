using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Threading.Tasks;
using Yunify.Auth.Server.Model;

namespace Yunify.Auth.Server.Context
{
    public class UserDbContext : IdentityDbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }

        public DbSet<UserModel> Users { get; set; }
        public DbSet<SerialModel> Serials { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<UserModel>().HasKey(m => m.UserName);
            builder.Entity<SerialModel>().HasKey(m => m.Id);

            base.OnModelCreating(builder);
        }

        public async Task Seed()
        {
            if (!await Users.AnyAsync())
            {
                base.AddRange(Config.GetUsers());
                base.SaveChanges();
            }

            if(!await Serials.AnyAsync())
            {
                base.AddRange(Config.GetSerials());
                base.SaveChanges();
            }
        }
    }

    public class UserDbContextFactory : IDesignTimeDbContextFactory<UserDbContext>
    {
        public UserDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<UserDbContext>();

            builder.UseSqlite("Data Source=db.identityconfig.sqlite");

            return new UserDbContext(builder.Options);
        }
    }
}
