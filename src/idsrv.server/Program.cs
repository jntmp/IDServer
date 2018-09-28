using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace idsrv.server
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHost(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHost(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args).UseStartup<Startup>();
        }
    }
}