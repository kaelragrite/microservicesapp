using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Ordering.API.Extensions;

namespace Ordering.API
{
    public class Program
    {
        public static void Main(string[] args) => 
            CreateHostBuilder(args)
                .Build()
                .CreateAndSeedDatabase()
                .Run();

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());
    }
}
