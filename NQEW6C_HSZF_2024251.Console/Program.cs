using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NQEW6C_HSZF_2024251.Application;
using NQEW6C_HSZF_2024251.Persistence.MsSql;
using System;
using System.Threading.Tasks;

namespace NQEW6C_HSZF_2024251
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Starting Application...");

            var host = Host.CreateDefaultBuilder()
                       .ConfigureServices((hostContext, services) =>
                       {
                           services.AddDbContext<AppDBContext>();

                           services.AddScoped<IF1DataProvider, F1DataProvider>();
                           services.AddScoped<IF1Service, F1Service>();

                           services.AddTransient<DatabaseSeeder>();
                           services.AddTransient<Menu>();
                           services.AddTransient<ToConsole>();
                       })
                       .Build();




            using (var serviceScope = host.Services.CreateScope())
            {
                var services = serviceScope.ServiceProvider;

                try
                {
                    var menu = services.GetRequiredService<Menu>();

                    await menu.ShowMainMenuAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Hiba történt: {ex.Message}");
                }
            }
        }
    }
}
