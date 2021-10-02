using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using WebStore.Data;

namespace WebStore
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host_builder = CreateHostBuilder(args);
            var host = host_builder.Build();

            using (var scope = host.Services.CreateScope())
            {
                var initializer = scope.ServiceProvider.GetRequiredService<WebStoreDbInitializer>();
                await initializer.InitializeAsync();
            }
            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(host =>
                    host.UseStartup<Startup>()
                )
            ;
    }
}
